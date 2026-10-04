using System.Text;
using System.Text.Json;
using JobHuntOS.Application.DTOs.Analysis;
using JobHuntOS.Application.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace JobHuntOS.Infrastructure.ExternalServices;

public class ClaudeApiService: IClaudeApiService
{
    private readonly HttpClient _httpClient;
    private readonly IConfiguration _configuration;
    private readonly ILogger<ClaudeApiService> _logger;

    public ClaudeApiService(HttpClient httpClient, 
        IConfiguration configuration, 
        ILogger<ClaudeApiService> logger)
    {
        _httpClient = httpClient;
        _configuration = configuration;
        _logger = logger;
    }

    public async Task<ClaudeAnalysisResult> AnalyseAsync(string cvContent, string jobDescription)
    {
        var apiKey = _configuration["Claude:ApiKey"];
        var model = _configuration["Claude:Model"];
        var maxTokens = int.Parse(_configuration["Claude:MaxTokens"] ?? "1024");

        var prompt = BuildPrompt(cvContent, jobDescription);

        var requestBody = new
        {
            model,
            max_tokens = maxTokens,
            messages = new[]
            {
                new { role = "user", content = prompt }
            }

        };
        var request = new HttpRequestMessage(HttpMethod.Post, "https://api.anthropic.com/v1/messages");
        request.Headers.Add("x-api-key", apiKey);
        request.Headers.Add("anthropic-version", "2023-06-01");
        request.Content = new StringContent(
            JsonSerializer.Serialize(requestBody),
            Encoding.UTF8,
            "application/json");

        var response = await _httpClient.SendAsync(request);
        response.EnsureSuccessStatusCode();

        var responseBody = await response.Content.ReadAsStringAsync();
        return ParseResponse(responseBody);
    }

    private static string BuildPrompt(string cvContent, string jobDescription)
    {
        return $$"""
                 You are an expert technical recruiter and career coach.
                 Analyse the fit between the candidate's CV and the job description below.

                 ## CV
                 {{cvContent}}

                 ## Job Description
                 {{jobDescription}}

                 Respond ONLY with valid JSON in this exact schema:
                 {
                   "fitScore": <integer 0-100>,
                   "matchingSkills": ["skill1", "skill2"],
                   "missingSkills": ["skill1", "skill2"],
                   "recommendations": "<2-3 sentences on how to tailor the application>"
                 }

                 No preamble. No markdown. Raw JSON only.
                 """;
    }

    private static ClaudeAnalysisResult ParseResponse(string responseBody)
    {
        using var doc = JsonDocument.Parse(responseBody);
        var content = doc.RootElement
            .GetProperty("content")[0]
            .GetProperty("text")
            .GetString() ?? string.Empty;


        return JsonSerializer.Deserialize<ClaudeAnalysisResult>(content, new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        }) ?? throw new InvalidOperationException("Failed to parse Claude response");
    }
}

using FluentAssertions;
using JobHuntOS.Application.DTOs.Analysis;
using JobHuntOS.Application.Exceptions;
using JobHuntOS.Application.Interfaces;
using JobHuntOS.Application.Services;
using JobHuntOS.Domain.Entities;
using Moq;

namespace JobHuntOS.Application.Tests.Services;

public class AnalysisServiceTests
{
    private readonly Mock<IClaudeApiService> _claudeMock = new();
    private readonly Mock<ICvRepository> _cvRepoMock = new();
    private readonly Mock<IAnalysisRepository> _analysisRepoMock = new();
    private readonly Mock<IApplicationRepository> _appRepoMock = new();
    private readonly Mock<ICurrentUserService> _currentUserMock = new();
    private readonly Guid _userId = Guid.NewGuid();
    private readonly AnalysisService _service;

    public AnalysisServiceTests()
    {
        _currentUserMock.Setup(c => c.UserId).Returns(_userId);

        // Constructor order must match AnalysisService:
        // claude, cvRepo, analysisRepo, currentUser, applicationRepo
        _service = new AnalysisService(
            _claudeMock.Object,
            _cvRepoMock.Object,
            _analysisRepoMock.Object,
            _currentUserMock.Object,
            _appRepoMock.Object);
    }

    [Fact]
    public async Task AnalyseAsync_ShouldThrow_WhenDailyLimitReached()
    {
        _analysisRepoMock
            .Setup(r => r.CountSinceAsync(_userId, It.IsAny<DateTime>()))
            .ReturnsAsync(AnalysisService.DailyLimit);

        var act = () => _service.AnalyseAsync(new AnalysisRequestDto { JobDescription = "x" });

        await act.Should().ThrowAsync<RateLimitExceededException>();

        // No paid call was made
        _claudeMock.Verify(
            c => c.AnalyseAsync(It.IsAny<string>(), It.IsAny<string>()),
            Times.Never);
    }

    [Fact]
    public async Task AnalyseAsync_ShouldThrow_WhenNoCvUploaded()
    {
        _analysisRepoMock
            .Setup(r => r.CountSinceAsync(_userId, It.IsAny<DateTime>()))
            .ReturnsAsync(0);
        _cvRepoMock
            .Setup(r => r.GetCurrentAsync(_userId))
            .ReturnsAsync((UserCV?)null);

        var act = () => _service.AnalyseAsync(new AnalysisRequestDto { JobDescription = "x" });

        await act.Should().ThrowAsync<KeyNotFoundException>();
        _claudeMock.Verify(
            c => c.AnalyseAsync(It.IsAny<string>(), It.IsAny<string>()),
            Times.Never);
    }

    [Fact]
    public async Task AnalyseAsync_ShouldSaveResultOwnedByCurrentUser()
    {
        _analysisRepoMock
            .Setup(r => r.CountSinceAsync(_userId, It.IsAny<DateTime>()))
            .ReturnsAsync(0);
        _cvRepoMock
            .Setup(r => r.GetCurrentAsync(_userId))
            .ReturnsAsync(new UserCV { Id = Guid.NewGuid(), UserId = _userId, Content = "my cv" });
        _claudeMock
            .Setup(c => c.AnalyseAsync("my cv", "a job"))
            .ReturnsAsync(new ClaudeAnalysisResult
            {
                FitScore = 80,
                MatchingSkills = new List<string> { "C#" },
                MissingSkills = new List<string>(),
                Recommendations = "Good fit"
            });

        var result = await _service.AnalyseAsync(new AnalysisRequestDto { JobDescription = "a job" });

        result.FitScore.Should().Be(80);

        // The saved row must belong to the caller, never Guid.Empty
        _analysisRepoMock.Verify(
            r => r.AddAsync(It.Is<AnalysisResult>(a => a.UserId == _userId)),
            Times.Once);
        _analysisRepoMock.Verify(r => r.SaveChangesAsync(), Times.Once);
    }
}
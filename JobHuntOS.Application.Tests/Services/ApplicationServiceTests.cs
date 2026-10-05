using FluentAssertions;
using JobHuntOS.Application.DTOs.Application;
using JobHuntOS.Application.Interfaces;
using JobHuntOS.Application.Services;
using JobHuntOS.Domain.Entities;
using JobHuntOS.Domain.Enums;
using Moq;

namespace JobHuntOS.Application.Tests.Services;

public class ApplicationServiceTests
{
    private readonly Mock<IApplicationRepository> _repositoryMock;
    private readonly ApplicationService _service;

    public ApplicationServiceTests()
    {
        _repositoryMock = new Mock<IApplicationRepository>();
        _service = new ApplicationService(_repositoryMock.Object);
    }

    [Fact]
    public async Task CreateAsync_ShouldCreateApplication_WithCorrectFollowUpDate()
    {
        // Arrange
        var dto = new CreateApplicationDto
        {
            CompanyName = "Google",
            JobTitle = "Software Engineer",
            AppliedDate = new DateTime(2026, 10, 1),
            FollowUpDays = 7
        };

        _repositoryMock
            .Setup(r => r.AddAsync(It.IsAny<JobApplication>()))
            .Returns(Task.CompletedTask);

        _repositoryMock
            .Setup(r => r.SaveChangesAsync())
            .ReturnsAsync(1);

        // Act
        var result = await _service.CreateAsync(dto);

        // Assert
        result.CompanyName.Should().Be("Google");
        result.JobTitle.Should().Be("Software Engineer");
        result.FollowUpDate.Should().Be(new DateTime(2026, 10, 8));
        result.Status.Should().Be(ApplicationStatus.Applied);
    }

    [Fact]
    public async Task GetByIdAsync_ShouldReturnNull_WhenApplicationNotFound()
    {
        // Arrange
        var id = Guid.NewGuid();

        _repositoryMock
            .Setup(r => r.GetByIdAsync(id))
            .ReturnsAsync((JobApplication?)null);

        // Act
        var result = await _service.GetByIdAsync(id);

        // Assert
        result.Should().BeNull();
    }

    [Fact]
    public async Task DeleteAsync_ShouldReturnFalse_WhenApplicationNotFound()
    {
        // Arrange
        var id = Guid.NewGuid();

        _repositoryMock
            .Setup(r => r.GetByIdAsync(id))
            .ReturnsAsync((JobApplication?)null);

        // Act
        var result = await _service.DeleteAsync(id);

        // Assert
        result.Should().BeFalse();
    }

    [Fact]
    public async Task DeleteAsync_ShouldReturnTrue_WhenApplicationExists()
    {
        // Arrange
        var id = Guid.NewGuid();
        var application = new JobApplication { Id = id, CompanyName = "Google" };

        _repositoryMock
            .Setup(r => r.GetByIdAsync(id))
            .ReturnsAsync(application);

        _repositoryMock
            .Setup(r => r.DeleteAsync(application))
            .Returns(Task.CompletedTask);

        _repositoryMock
            .Setup(r => r.SaveChangesAsync())
            .ReturnsAsync(1);

        // Act
        var result = await _service.DeleteAsync(id);

        // Assert
        result.Should().BeTrue();
    }
}
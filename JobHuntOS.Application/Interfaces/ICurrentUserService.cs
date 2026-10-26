namespace JobHuntOS.Application.Interfaces;

public interface ICurrentUserService
{
    Guid UserId { get; }
}
namespace Catalog.Worker.Application.Common.Interfaces;

public interface ICurrentUserService
{
    string? UserId { get; }
}
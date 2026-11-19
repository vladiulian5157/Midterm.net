namespace RoomHub.Api.Domain;

/// <summary>Commits everything the repositories added or changed, in one transaction.</summary>
public interface IUnitOfWork
{
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}

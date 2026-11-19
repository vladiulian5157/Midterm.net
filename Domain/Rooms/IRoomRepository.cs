namespace RoomHub.Api.Domain.Rooms;

public interface IRoomRepository
{
    Task<Room?> GetByIdAsync(Guid id, CancellationToken cancellationToken);
    void Add(Room room);
}

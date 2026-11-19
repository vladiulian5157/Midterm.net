using Microsoft.EntityFrameworkCore;
using RoomHub.Api.Domain.Rooms;

namespace RoomHub.Api.Infrastructure.Persistence;

public sealed class RoomRepository(RoomHubDbContext db) : IRoomRepository
{
    public Task<Room?> GetByIdAsync(Guid id, CancellationToken cancellationToken) =>
        db.Rooms.FirstOrDefaultAsync(r => r.Id == id, cancellationToken);

    public void Add(Room room) => db.Rooms.Add(room);
}

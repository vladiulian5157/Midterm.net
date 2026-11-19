using Microsoft.EntityFrameworkCore;
using RoomHub.Api.Domain.Bookings;

namespace RoomHub.Api.Infrastructure.Persistence;

public sealed class BookingRepository(RoomHubDbContext db) : IBookingRepository
{
    public Task<Booking?> GetByIdAsync(Guid id, CancellationToken cancellationToken) =>
        db.Bookings.FirstOrDefaultAsync(b => b.Id == id, cancellationToken);

    public async Task<IReadOnlyList<Booking>> ListActiveForRoomAsync(Guid roomId, CancellationToken cancellationToken)
    {
        var active = await db.Bookings
            .Where(b => b.RoomId == roomId && b.Status == BookingStatus.Active)
            .ToListAsync(cancellationToken);

        return active;
    }

    public async Task<IReadOnlyList<Booking>> ListActiveForParticipantOnDayAsync(
        string bookedBy, DateTime dayUtc, CancellationToken cancellationToken)
    {
        var dayStart = DateTime.SpecifyKind(dayUtc.Date, DateTimeKind.Utc);
        var dayEnd = dayStart.AddDays(1);

        var active = await db.Bookings
            .Where(b => b.BookedBy == bookedBy
                        && b.Status == BookingStatus.Active
                        && b.StartUtc >= dayStart
                        && b.StartUtc < dayEnd)
            .ToListAsync(cancellationToken);

        return active;
    }

    public void Add(Booking booking) => db.Bookings.Add(booking);
}

namespace RoomHub.Api.Domain.Bookings;

public interface IBookingRepository
{
    Task<Booking?> GetByIdAsync(Guid id, CancellationToken cancellationToken);
    Task<IReadOnlyList<Booking>> ListActiveForRoomAsync(Guid roomId, CancellationToken cancellationToken);

    /// <summary>Active bookings of one participant, in all rooms, whose start is on the given UTC date.</summary>
    Task<IReadOnlyList<Booking>> ListActiveForParticipantOnDayAsync(
        string bookedBy, DateTime dayUtc, CancellationToken cancellationToken);

    void Add(Booking booking);
}

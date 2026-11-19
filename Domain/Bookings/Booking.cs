using RoomHub.Api.Domain.Rooms;
using RoomHub.Api.SharedKernel;

namespace RoomHub.Api.Domain.Bookings;

public sealed class Booking
{
    // R6: 4 hours is the maximum, exactly 4 hours is allowed.
    public static readonly TimeSpan MaxDuration = TimeSpan.FromHours(4);

    private Booking() { } // for EF Core

    public Guid Id { get; private set; }
    public Guid RoomId { get; private set; }
    public string BookedBy { get; private set; } = string.Empty;
    public DateTime StartUtc { get; private set; }
    public DateTime EndUtc { get; private set; }
    public int Attendees { get; private set; }
    public BookingStatus Status { get; private set; }

    /// <summary>
    /// All booking rules live here (R2-R7). "Now" is given by the caller (from TimeProvider),
    /// and the existing bookings of the room are given by the caller (from the repository),
    /// so the domain stays free of infrastructure and of the system clock.
    /// </summary>
    public static Result<Booking> Create(
        Room room,
        string? bookedBy,
        DateTime startUtc,
        DateTime endUtc,
        int attendees,
        DateTime nowUtc,
        IEnumerable<Booking> roomBookings)
    {
        if (string.IsNullOrWhiteSpace(bookedBy))                     // R2
            return BookingErrors.BookedByRequired;

        var start = ToUtc(startUtc);
        var end = ToUtc(endUtc);

        if (end <= start)                                            // R3
            return BookingErrors.InvalidRange;

        if (attendees <= 0)                                          // R4
            return BookingErrors.InvalidAttendees;

        if (attendees > room.Capacity)                               // R4
            return BookingErrors.AttendeesExceedCapacity;

        if (start < ToUtc(nowUtc))                                   // R5
            return BookingErrors.StartInThePast;

        if (end - start > MaxDuration)                               // R6
            return BookingErrors.TooLong;

        if (roomBookings.Any(b => b.RoomId == room.Id && b.OverlapsWith(start, end)))   // R7
            return BookingErrors.RoomOccupied;

        return new Booking
        {
            Id = Guid.NewGuid(),
            RoomId = room.Id,
            BookedBy = bookedBy.Trim(),
            StartUtc = start,
            EndUtc = end,
            Attendees = attendees,
            Status = BookingStatus.Active
        };
    }

    // R7: only ACTIVE bookings block an interval, and intervals that only touch do not overlap
    // (strict comparisons).
    public bool OverlapsWith(DateTime startUtc, DateTime endUtc) =>
        Status == BookingStatus.Active && StartUtc < endUtc && startUtc < EndUtc;

    // R8: Active -> Cancelled. Cancelling twice is a conflict.
    public Result Cancel()
    {
        if (Status == BookingStatus.Cancelled)
            return Result.Failure(BookingErrors.AlreadyCancelled);

        Status = BookingStatus.Cancelled;
        return Result.Success();
    }

    private static DateTime ToUtc(DateTime value) => value.Kind switch
    {
        DateTimeKind.Utc => value,
        DateTimeKind.Local => value.ToUniversalTime(),
        _ => DateTime.SpecifyKind(value, DateTimeKind.Utc)
    };
}

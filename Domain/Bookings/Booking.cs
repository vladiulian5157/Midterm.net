using RoomHub.Api.Domain.Rooms;
using RoomHub.Api.SharedKernel;

namespace RoomHub.Api.Domain.Bookings;

public sealed class Booking
{
    // R9: at most 2 active bookings per participant per UTC day (all rooms together).
    public const int MaxActivePerParticipantPerDay = 2;

    // R11: cancelling is allowed only if the booking starts at least 60 minutes from now.
    public static readonly TimeSpan MinCancelNotice = TimeSpan.FromMinutes(60);

    private Booking() { } // for EF Core

    public Guid Id { get; private set; }
    public Guid RoomId { get; private set; }
    public string BookedBy { get; private set; } = string.Empty;
    public DateTime StartUtc { get; private set; }
    public DateTime EndUtc { get; private set; }
    public int Attendees { get; private set; }
    public BookingStatus Status { get; private set; }

    /// <summary>
    /// All booking rules live here (R2-R7, R9, R10). "Now", the existing bookings of the room and the
    /// existing bookings of the participant for that day are given by the caller, so the domain stays free
    /// of infrastructure and of the system clock.
    /// </summary>
    public static Result<Booking> Create(
        Room room,
        string? bookedBy,
        DateTime startUtc,
        DateTime endUtc,
        int attendees,
        DateTime nowUtc,
        IEnumerable<Booking> roomBookings,
        IEnumerable<Booking> participantBookingsOnDay)
    {
        if (string.IsNullOrWhiteSpace(bookedBy))                     // R2
            return BookingErrors.BookedByRequired;

        var start = NormalizeUtc(startUtc);
        var end = NormalizeUtc(endUtc);
        var name = bookedBy.Trim();

        if (end <= start)                                            // R3
            return BookingErrors.InvalidRange;

        if (attendees <= 0)                                          // R4
            return BookingErrors.InvalidAttendees;

        if (attendees > room.Capacity)                               // R4
            return BookingErrors.AttendeesExceedCapacity;

        if (start < NormalizeUtc(nowUtc))                            // R5
            return BookingErrors.StartInThePast;

        if (end - start > room.MaxBookingDuration)                   // R6 + R10
            return BookingErrors.TooLong;

        if (roomBookings.Any(b => b.RoomId == room.Id && b.OverlapsWith(start, end)))   // R7
            return BookingErrors.RoomOccupied;

        var sameDayActive = participantBookingsOnDay.Count(b =>      // R9
            b.Status == BookingStatus.Active
            && b.BookedBy == name
            && b.StartUtc.Date == start.Date);
        if (sameDayActive >= MaxActivePerParticipantPerDay)
            return BookingErrors.DailyLimitReached;

        return new Booking
        {
            Id = Guid.NewGuid(),
            RoomId = room.Id,
            BookedBy = name,
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
    // R11: only if the booking starts at least 60 minutes from now (exactly 60 is allowed).
    public Result Cancel(DateTime nowUtc)
    {
        if (Status == BookingStatus.Cancelled)
            return Result.Failure(BookingErrors.AlreadyCancelled);

        if (StartUtc < NormalizeUtc(nowUtc) + MinCancelNotice)
            return Result.Failure(BookingErrors.CancelTooLate);

        Status = BookingStatus.Cancelled;
        return Result.Success();
    }

    /// <summary>Every date that enters the domain is treated as UTC (Local converted, Unspecified assumed UTC).</summary>
    public static DateTime NormalizeUtc(DateTime value) => value.Kind switch
    {
        DateTimeKind.Utc => value,
        DateTimeKind.Local => value.ToUniversalTime(),
        _ => DateTime.SpecifyKind(value, DateTimeKind.Utc)
    };
}

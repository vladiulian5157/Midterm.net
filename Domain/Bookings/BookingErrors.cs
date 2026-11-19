using RoomHub.Api.SharedKernel;

namespace RoomHub.Api.Domain.Bookings;

public static class BookingErrors
{
    public static readonly Error BookedByRequired =
        Error.Validation("InvalidBookedBy", "BookedBy is required.");

    public static readonly Error InvalidRange =
        Error.Validation("InvalidRange", "The end must be after the start.");

    public static readonly Error InvalidAttendees =
        Error.Validation("InvalidAttendees", "Attendees must be positive.");

    public static readonly Error AttendeesExceedCapacity =
        Error.Validation("AttendeesExceedCapacity", "The room does not have enough seats.");

    public static readonly Error StartInThePast =
        Error.Validation("StartInThePast", "A booking cannot start in the past.");

    public static readonly Error TooLong =
        Error.Validation("BookingTooLong", "The booking is longer than the maximum allowed for this room.");

    public static readonly Error RoomOccupied =
        Error.Conflict("RoomOccupied", "The room is occupied in this interval.");

    public static readonly Error AlreadyCancelled =
        Error.Conflict("BookingAlreadyCancelled", "The booking is already cancelled.");

    public static readonly Error DailyLimitReached =
        Error.Conflict("BookingDailyLimitReached", "A participant can have at most 2 active bookings per day.");

    public static readonly Error CancelTooLate =
        Error.Conflict("CancelTooLate", "A booking can be cancelled only if it starts at least 60 minutes from now.");

    public static readonly Error NotFound =
        Error.NotFound("BookingNotFound", "The booking does not exist.");
}

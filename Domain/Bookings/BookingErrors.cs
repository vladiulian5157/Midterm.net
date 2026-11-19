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
        Error.Validation("BookingTooLong", "A booking can last at most 4 hours.");

    public static readonly Error RoomOccupied =
        Error.Conflict("RoomOccupied", "The room is occupied in this interval.");

    public static readonly Error AlreadyCancelled =
        Error.Conflict("BookingAlreadyCancelled", "The booking is already cancelled.");

    public static readonly Error NotFound =
        Error.NotFound("BookingNotFound", "The booking does not exist.");
}

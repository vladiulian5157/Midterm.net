namespace RoomHub.Api.Features.Bookings.GetBooking;

public sealed record GetBookingQuery(Guid Id);

public sealed record GetBookingResponse(
    Guid Id,
    Guid RoomId,
    string BookedBy,
    DateTime StartUtc,
    DateTime EndUtc,
    int Attendees,
    string Status);

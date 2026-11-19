using RoomHub.Api.Domain.Bookings;
using RoomHub.Api.SharedKernel;

namespace RoomHub.Api.Features.Bookings.GetBooking;

public sealed class GetBookingHandler(IBookingRepository bookings) : IQueryHandler<GetBookingQuery, GetBookingResponse>
{
    public async Task<Result<GetBookingResponse>> Handle(GetBookingQuery query, CancellationToken cancellationToken)
    {
        var booking = await bookings.GetByIdAsync(query.Id, cancellationToken);
        if (booking is null)
            return BookingErrors.NotFound;

        return new GetBookingResponse(
            booking.Id,
            booking.RoomId,
            booking.BookedBy,
            booking.StartUtc,
            booking.EndUtc,
            booking.Attendees,
            booking.Status.ToString());
    }
}

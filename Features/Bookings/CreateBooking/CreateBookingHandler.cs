using RoomHub.Api.Domain;
using RoomHub.Api.Domain.Bookings;
using RoomHub.Api.Domain.Rooms;
using RoomHub.Api.SharedKernel;

namespace RoomHub.Api.Features.Bookings.CreateBooking;

public sealed class CreateBookingHandler(
    IRoomRepository rooms,
    IBookingRepository bookings,
    IUnitOfWork unitOfWork,
    TimeProvider timeProvider)
    : ICommandHandler<CreateBookingCommand, CreateBookingResponse>
{
    public async Task<Result<CreateBookingResponse>> Handle(CreateBookingCommand command, CancellationToken cancellationToken)
    {
        // The room is checked first: a missing room is 404 before any validation.
        var room = await rooms.GetByIdAsync(command.RoomId, cancellationToken);
        if (room is null)
            return RoomErrors.NotFound;

        var activeBookings = await bookings.ListActiveForRoomAsync(room.Id, cancellationToken);
        var nowUtc = timeProvider.GetUtcNow().UtcDateTime;

        var created = Booking.Create(
            room, command.BookedBy, command.StartUtc, command.EndUtc, command.Attendees, nowUtc, activeBookings);
        if (created.IsFailure)
            return created.Error;

        var booking = created.Value;
        bookings.Add(booking);
        await unitOfWork.SaveChangesAsync(cancellationToken);

        return new CreateBookingResponse(booking.Id, booking.RoomId, booking.Status.ToString());
    }
}

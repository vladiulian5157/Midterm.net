using RoomHub.Api.Domain;
using RoomHub.Api.Domain.Bookings;
using RoomHub.Api.SharedKernel;

namespace RoomHub.Api.Features.Bookings.CancelBooking;

public sealed class CancelBookingHandler(IBookingRepository bookings, IUnitOfWork unitOfWork, TimeProvider timeProvider)
    : ICommandHandler<CancelBookingCommand>
{
    public async Task<Result> Handle(CancelBookingCommand command, CancellationToken cancellationToken)
    {
        var booking = await bookings.GetByIdAsync(command.Id, cancellationToken);
        if (booking is null)
            return Result.Failure(BookingErrors.NotFound);

        var cancelled = booking.Cancel(timeProvider.GetUtcNow().UtcDateTime);
        if (cancelled.IsFailure)
            return cancelled;

        await unitOfWork.SaveChangesAsync(cancellationToken);
        return Result.Success();
    }
}

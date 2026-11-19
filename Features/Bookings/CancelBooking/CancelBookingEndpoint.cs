using RoomHub.Api.SharedKernel;

namespace RoomHub.Api.Features.Bookings.CancelBooking;

public static class CancelBookingEndpoint
{
    public static void MapCancelBooking(this IEndpointRouteBuilder app) =>
        app.MapPost("/api/bookings/{id:guid}/cancel", async (
            Guid id,
            ICommandHandler<CancelBookingCommand> handler,
            CancellationToken cancellationToken) =>
        {
            var result = await handler.Handle(new CancelBookingCommand(id), cancellationToken);

            return result.IsSuccess
                ? Results.NoContent()
                : result.Error.ToProblem();
        }).WithTags("Bookings");
}

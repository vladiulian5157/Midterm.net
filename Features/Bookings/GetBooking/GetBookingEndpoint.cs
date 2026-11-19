using RoomHub.Api.SharedKernel;

namespace RoomHub.Api.Features.Bookings.GetBooking;

public static class GetBookingEndpoint
{
    public static void MapGetBooking(this IEndpointRouteBuilder app) =>
        app.MapGet("/api/bookings/{id:guid}", async (
            Guid id,
            IQueryHandler<GetBookingQuery, GetBookingResponse> handler,
            CancellationToken cancellationToken) =>
        {
            var result = await handler.Handle(new GetBookingQuery(id), cancellationToken);

            return result.IsSuccess
                ? Results.Ok(result.Value)
                : result.Error.ToProblem();
        }).WithTags("Bookings");
}

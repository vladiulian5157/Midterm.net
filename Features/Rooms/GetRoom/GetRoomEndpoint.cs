using RoomHub.Api.SharedKernel;

namespace RoomHub.Api.Features.Rooms.GetRoom;

public static class GetRoomEndpoint
{
    public static void MapGetRoom(this IEndpointRouteBuilder app) =>
        app.MapGet("/api/rooms/{id:guid}", async (
            Guid id,
            IQueryHandler<GetRoomQuery, GetRoomResponse> handler,
            CancellationToken cancellationToken) =>
        {
            var result = await handler.Handle(new GetRoomQuery(id), cancellationToken);

            return result.IsSuccess
                ? Results.Ok(result.Value)
                : result.Error.ToProblem();
        }).WithTags("Rooms");
}

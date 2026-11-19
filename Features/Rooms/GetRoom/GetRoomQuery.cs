namespace RoomHub.Api.Features.Rooms.GetRoom;

public sealed record GetRoomQuery(Guid Id);

public sealed record GetRoomResponse(Guid Id, string Name, int Capacity);

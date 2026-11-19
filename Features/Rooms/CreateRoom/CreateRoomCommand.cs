namespace RoomHub.Api.Features.Rooms.CreateRoom;

public sealed record CreateRoomCommand(string? Name, int Capacity);

public sealed record CreateRoomResponse(Guid Id, string Name, int Capacity);

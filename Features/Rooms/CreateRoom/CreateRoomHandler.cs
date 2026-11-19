using RoomHub.Api.Domain;
using RoomHub.Api.Domain.Rooms;
using RoomHub.Api.SharedKernel;

namespace RoomHub.Api.Features.Rooms.CreateRoom;

public sealed class CreateRoomHandler(IRoomRepository rooms, IUnitOfWork unitOfWork)
    : ICommandHandler<CreateRoomCommand, CreateRoomResponse>
{
    public async Task<Result<CreateRoomResponse>> Handle(CreateRoomCommand command, CancellationToken cancellationToken)
    {
        var created = Room.Create(command.Name, command.Capacity);
        if (created.IsFailure)
            return created.Error;

        var room = created.Value;
        rooms.Add(room);
        await unitOfWork.SaveChangesAsync(cancellationToken);

        return new CreateRoomResponse(room.Id, room.Name, room.Capacity);
    }
}

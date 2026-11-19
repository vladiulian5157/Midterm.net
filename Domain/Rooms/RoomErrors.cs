using RoomHub.Api.SharedKernel;

namespace RoomHub.Api.Domain.Rooms;

public static class RoomErrors
{
    public static readonly Error NameRequired =
        Error.Validation("InvalidName", "Room name is required.");

    public static readonly Error CapacityMustBePositive =
        Error.Validation("InvalidCapacity", "Capacity must be positive.");

    public static readonly Error NotFound =
        Error.NotFound("RoomNotFound", "The room does not exist.");
}

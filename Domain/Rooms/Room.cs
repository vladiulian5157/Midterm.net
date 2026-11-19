using RoomHub.Api.SharedKernel;

namespace RoomHub.Api.Domain.Rooms;

public sealed class Room
{
    private Room() { } // for EF Core

    public Guid Id { get; private set; }
    public string Name { get; private set; } = string.Empty;
    public int Capacity { get; private set; }

    // R1: non-empty name, strictly positive capacity.
    public static Result<Room> Create(string? name, int capacity)
    {
        if (string.IsNullOrWhiteSpace(name))
            return RoomErrors.NameRequired;

        if (capacity <= 0)
            return RoomErrors.CapacityMustBePositive;

        return new Room
        {
            Id = Guid.NewGuid(),
            Name = name.Trim(),
            Capacity = capacity
        };
    }
}

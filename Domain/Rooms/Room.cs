using RoomHub.Api.SharedKernel;

namespace RoomHub.Api.Domain.Rooms;

public sealed class Room
{
    private Room() { } // for EF Core

    public Guid Id { get; private set; }
    public string Name { get; private set; } = string.Empty;
    public int Capacity { get; private set; }

    // R10: rooms with capacity > 10 allow at most 2 hours, the others keep the 4-hour limit (R6).
    // Exactly the limit is allowed.
    public TimeSpan MaxBookingDuration =>
        Capacity > 10 ? TimeSpan.FromHours(2) : TimeSpan.FromHours(4);

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

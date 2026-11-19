namespace RoomHub.Api.SharedKernel;

/// <summary>A command that changes state and returns data.</summary>
public interface ICommandHandler<in TCommand, TResponse>
{
    Task<Result<TResponse>> Handle(TCommand command, CancellationToken cancellationToken);
}

/// <summary>A command that changes state and returns nothing.</summary>
public interface ICommandHandler<in TCommand>
{
    Task<Result> Handle(TCommand command, CancellationToken cancellationToken);
}

namespace RoomHub.Api.SharedKernel;

/// <summary>A read-only request. It never changes state.</summary>
public interface IQueryHandler<in TQuery, TResponse>
{
    Task<Result<TResponse>> Handle(TQuery query, CancellationToken cancellationToken);
}

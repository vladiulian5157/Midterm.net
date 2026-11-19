using RoomHub.Api.Features.Bookings.CancelBooking;
using RoomHub.Api.Features.Bookings.CreateBooking;
using RoomHub.Api.Features.Bookings.GetBooking;
using RoomHub.Api.Features.Rooms.CreateRoom;
using RoomHub.Api.Features.Rooms.GetRoom;
using RoomHub.Api.SharedKernel;

namespace RoomHub.Api.Features;

public static class FeatureRegistration
{
    // Explicit registration (no assembly scanning): easy to read and to explain.
    public static IServiceCollection AddFeatures(this IServiceCollection services)
    {
        services.AddScoped<ICommandHandler<CreateRoomCommand, CreateRoomResponse>, CreateRoomHandler>();
        services.AddScoped<IQueryHandler<GetRoomQuery, GetRoomResponse>, GetRoomHandler>();
        services.AddScoped<ICommandHandler<CreateBookingCommand, CreateBookingResponse>, CreateBookingHandler>();
        services.AddScoped<IQueryHandler<GetBookingQuery, GetBookingResponse>, GetBookingHandler>();
        services.AddScoped<ICommandHandler<CancelBookingCommand>, CancelBookingHandler>();
        return services;
    }

    public static IEndpointRouteBuilder MapFeatures(this IEndpointRouteBuilder app)
    {
        app.MapCreateRoom();
        app.MapGetRoom();
        app.MapCreateBooking();
        app.MapGetBooking();
        app.MapCancelBooking();
        return app;
    }
}

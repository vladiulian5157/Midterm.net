# RoomHub

Study-room booking API: vertical slice, rich domain, `Result<T>`, handlers, EF Core + SQLite
(Repository + Unit of Work). The assignment is in `ASSIGNMENT.md`.

## Run

```bash
dotnet run --project src/RoomHub.Api
```

Starts on `http://localhost:5090`. The SQLite file `roomhub.db` is created in the folder you run from
(it is git-ignored). Test requests: `roomhub.http`.

## Structure

```
src/RoomHub.Api
  SharedKernel/      Result, Error, ICommandHandler, IQueryHandler, Error -> HTTP mapping
  Domain/            Room, Booking (rules R1-R8), errors, IRoomRepository, IBookingRepository, IUnitOfWork
  Features/          one folder per use case: command/query + handler + endpoint
    Rooms/CreateRoom  Rooms/GetRoom
    Bookings/CreateBooking  Bookings/GetBooking  Bookings/CancelBooking
  Infrastructure/    EF Core DbContext, configurations, repositories, UnitOfWork (the only place that knows EF)
```

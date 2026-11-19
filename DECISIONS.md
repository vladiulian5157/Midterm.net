# DECISIONS

Answer **yourself**, in your own words, in 3–5 sentences per question.
Refer to concrete files and classes from your own repo (for example `Features/Bookings/CreateBooking/...`).
An unanswered question gives 0 points.

## 1. A rule you moved
Pick one business rule (R1–R8). In which file did it live in the starter and in which file does it live now? Why there?

R7 (suprapunerea) era în BookingService.Create, intr-un foreach. Acum e în Booking.OverlapsWith și Booking.Create, pentru ca domeniul trebuie să apere singur regulile, indiferent de cine il apeleaza.

## 2. Result instead of an exception
„Camera e ocupata“ e un caz normal de business, nu ceva excepțional. Excepțiile sunt ascunse in flux (nu se vad în semnatura) și costisitoare. La noi handlerul întoarce Error (tip Conflict), iar ErrorHttpExtensions.ToProblem() il transforma in 409 ProblemDetails. Am scos ExceptionMiddleware și BusinessException.


## 3. Who knows about EF Core?
Doar Infrastructure/Persistence. Domeniul și handlerele vad doar interfețele IRoomRepository, IBookingRepository, IUnitOfWork. Caștigi testabilitate și poti schimba baza de date. Platești mai multe fișiere și un strat în plus.


## 4. The flow of a request
Draw in text (ASCII arrows) the path of a `POST /api/rooms/{roomId}/bookings` request through your project, with the **real** names of files and classes, from the endpoint to the database.

POST /api/rooms/{roomId}/bookings → CreateBookingEndpoint → CreateBookingHandler → RoomRepository.GetByIdAsync → BookingRepository.ListActiveForRoomAsync → Booking.Create → BookingRepository.Add → UnitOfWork.SaveChangesAsync → RoomHubDbContext → SQLite, iar la întoarcere Result → ToProblem() sau 201.

## 5. What you rejected from what the AI gave you
Give a concrete example of code or an idea proposed by the AI that you corrected or rejected, and say why.

AI-ul vrea de obicei DateTime.UtcNow în loc de TimeProvider, sau DbContext direct în handler, sau setteri publici pe entități.

## 6. If you had 30 more minutes
What would you improve and what risk did you leave in the project?

Doua cereri simultane pot trece amandoua de verificarea de suprapunere (check-then-insert, fara constrangere in baza de date). Aș mai adauga teste automate și migrari EF in loc de EnsureCreated.

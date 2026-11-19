# BUGS

The starter contains behaviors that contradict `ASSIGNMENT.md`. Write here every discrepancy you found.
Fill it in yourself, in your own words. There is no required number: what counts is that they are real.

| Which rule from ASSIGNMENT.md is violated (R1–R8) | Where the problem was in the starter (file, approximate line) | How you discovered it (test, reading, AI) | What you changed |
|---|---|---|---|
| _(fill in)_ | _(fill in)_ | _(fill in)_ | _(fill in)_ |


R7	BookingService.Create, foreach	Folosea <=, deci intervalele care doar se ating dadeau 409	Comparații stricte (<) în Booking.OverlapsWith
R7/R8	BookingService.Create	Rezervarile anulate înca blocau intervalul	OverlapsWith cere Status == Active
R8	BookingService.Cancel	Anula necondiționat, deci a doua anulare dădea 204 în loc de 409	Booking.Cancel() întoarce AlreadyCancelled
R5	lipsea	Se putea rezerva în trecut	Verificare cu now din TimeProvider
R6	lipsea	Nicio limita de durata	Maxim 4 ore, exact 4 ore e permis
Persistența	RoomRepository/BookingRepository	Date in memorie, pierdute la restart	EF Core + SQLite
Contract	efect secundar al SQLite	Datele citite din SQLite pierd DateTimeKind, deci JSON-ul n-ar mai avea Z	ValueConverter in BookingConfiguration
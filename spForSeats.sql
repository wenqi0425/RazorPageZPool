create proc spGetSeatsAvailableByRideID
@RideID int
as
BEgin
select Rides.SeatsAvailable, (select count(*) from bookings where Bookings.RideID= rides.RideID and Bookings.BookingStatus='Accepted') as BookedSeats, Rides.SeatsAvailable - (select count(*) from bookings where Bookings.RideID= rides.RideID and Bookings.BookingStatus='Accepted') as SeatsLeft
from rides
where RideID = @RideID
end


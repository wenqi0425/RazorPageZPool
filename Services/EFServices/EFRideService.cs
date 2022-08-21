using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ZPool.Models;
using ZPool.Pages.Rides;
using ZPool.Services.Interfaces;

namespace ZPool.Services.EFServices.RideService
{
    public class EFRideService : IRideService
    {
        AppDbContext _context;
        
        public EFRideService(AppDbContext context)
        {
            _context = context;
        }

        public void AddRide(Ride ride)
        {
            _context.Rides.Add(ride);
            _context.SaveChanges();
        }

        public void DeleteRide(Ride ride)
        {
            _context.Rides.Remove(ride);
            _context.SaveChanges();
        }

        public void EditRide(Ride ride)
        {
            // a future ride changes to the passed time is allowed, but not show in Rides
            _context.Rides.Update(ride);
            _context.SaveChanges();  
        }

        public IEnumerable<Ride> GetAllRides()
        {
            return _context.Rides
                .Include(r => r.Car)
                .ThenInclude(c => c.AppUser);
        }

        public Ride GetRide(int rideId)
        {
            return _context.Rides
                .Include(r=>r.Car)
                .ThenInclude(c=>c.AppUser)
                .FirstOrDefault(r=>r.RideID==rideId);
        }

        // select car to create Ride
        public IEnumerable<Car> GetRegisteredCars(int userId)
        {
            return _context.Cars.AsNoTracking().Where(c => c.AppUserID == userId);
        }

        public IEnumerable<Ride> FilterRides(RideCriteriaInputModel criteria)
        {
            return _context.Rides
                .Include(r=>r.Car)
                .AsNoTracking()
                .AsEnumerable()
                // return full list or results
                // CheckDeparture(x, y) ： Func<int, int, bool>
                .Where(ride=>CheckDeparture(ride, criteria.DepartureLocation))  
                .Where(ride=>CheckDestination(ride, criteria.DestinationLocation))
                .Where(ride=>CheckStartTime(ride, criteria.StartTime))
                .OrderBy(r=>r.StartTime);; // now to the future
        }

        //public IEnumerable<Ride> GetRidesForReview(int revieweeId, int reviewerId)
        //{
        //    return _context.Rides.Include(r => r.Car)
        //        .Where(r => r.Car.AppUserID == revieweeId)
        //        .Include(r => r.Bookings)
        //        .Where(r => r.StartTime < DateTime.Now);
        //}

        #region FilterHelperMethods

        private bool CheckDeparture(Ride ride, string location)
        {
            if (string.IsNullOrEmpty(location)) return true;  // no searching criteria input
            else if (ride.DepartureLocation.ToLower().Contains(location.ToLower())) return true;
            else return false;
        }

        private bool CheckDestination(Ride ride, string location)
        {
            if (string.IsNullOrEmpty(location)) return true;
            else if (ride.DestinationLocation.ToLower().Contains(location.ToLower())) return true;
            else return false;
        }

        // +2 hours scope
        private bool CheckStartTime(Ride ride, DateTime searchTime)
        {
            bool inRange = (ride.StartTime >= searchTime.Subtract(new TimeSpan(2, 0, 0)) &&
                            ride.StartTime <= searchTime.Add(new TimeSpan(2, 0, 0)));
            return inRange ? true : false;
        }

        #endregion

        // Method for Profile page
        public IEnumerable<Ride> GetRidesByUser(AppUser user)
        {
            // get the logged user's car list
            IEnumerable<int> lst = _context.Cars
                .Where(c => c.AppUserID.Equals(user.Id)).ToList().Select(c => c.CarID);
            
            // return all rides created by these cars
            return from r in _context.Rides where lst.Contains(r.CarID) select r;
        }

        public int SeatsLeft(int rideId)
        {
            int acceptedBookings = _context.Bookings
                .Where(b => b.RideID == rideId)
                .Count(b => b.BookingStatus == "Accepted");

            int seatsLeft = _context.Rides.Find(rideId).SeatsAvailable - acceptedBookings;
            return seatsLeft;
        }

        public IEnumerable<Ride> GetRidesForReview(int revieweeId, int reviewerId)
        {
            return _context.Rides.Include(r => r.Car)
                .Where(r => r.Car.AppUserID == revieweeId)
                .Include(r => r.Bookings).AsEnumerable().Where(r=> CheckBookingsForReview(r, reviewerId))
                .Where(r => r.StartTime < DateTime.Now);
        }

        private bool CheckBookingsForReview(Ride r, int id)
        {
            foreach (var booking in r.Bookings.AsEnumerable())
            {
                if (booking.AppUserID == id && booking.BookingStatus == "Accepted") return true;
            }
            return false;
        }
    }
}

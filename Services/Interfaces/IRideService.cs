using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ZPool.Models;
using ZPool.Pages.Rides;

namespace ZPool.Services.Interfaces
{
    public interface IRideService
    {
        void AddRide(Ride ride);
        void DeleteRide(Ride ride);
        Ride GetRide(int rideId);
        IEnumerable<Ride> GetAllRides();
        void EditRide(Ride ride);
        IEnumerable<Car> GetRegisteredCars(int userId);
        IEnumerable<Ride> FilterRides(RideCriteriaInputModel criteria);

        //Method for profile page
        IEnumerable<Ride> GetRidesByUser(AppUser user);
        int SeatsLeft(int rideId);
        IEnumerable<Ride> GetRidesForReview(int revieweeId, int reviewerId);

    }
}

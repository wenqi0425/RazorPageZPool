using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ZPool.Models;

namespace ZPool.Services.Interfaces
{
   public  interface IBookingService
    {
        Booking GetBookingsByID(int bookingId);
        bool AlreadyBooked(int rideId, int userId);
        void AddBooking(Booking booking);
        
        //Method to filter booking by logged in users in Bookings page
        IEnumerable<Booking> GetBookingsByDriversID(AppUser user);

        // Method for Profile Page
        IEnumerable<Booking> GetBookingsByUser(AppUser user);
        IEnumerable<Booking> GetBookingsByRideId(int rideId);
        void UpdateBookingStatus(int id, string bookingStatus);
        IEnumerable<Booking> GetBookingsByStatus(string statusFilter, AppUser user);
        IEnumerable<Booking> GetBookingsByStatusForDrivers(string statusFilter, AppUser user);


        //void DeleteBooking(Booking booking);  
        //void EditBooking(Booking booking);
        //IEnumerable<Booking> GetBookings();
    }
}

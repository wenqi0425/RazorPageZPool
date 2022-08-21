using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity.UI.Services;
using ZPool.Models;
using ZPool.Services.Interfaces;

namespace ZPool.Services.EFServices
{
    public class EFBookingService: IBookingService
    {
        private AppDbContext _context;
        private IMessageService _messageService;
        private IEmailSender _emailSender;
        private IRideService _rideService;

            public EFBookingService(AppDbContext context, 
            IMessageService smsService, 
            IEmailSender emailSender, 
            IRideService rideService)
        {
           _context = context;
           _messageService = smsService;
           _emailSender = emailSender;
           _rideService = rideService;
        }

        public void AddBooking(Booking booking)
        {
            if (!AlreadyBooked(booking.RideID, booking.AppUserID))  // passenger Id
            {
                _context.Bookings.Add(booking);
                _context.SaveChanges();
                SendMessageToDriver(booking);
                SendEmailToDriver(booking);
            }
        }

        public bool AlreadyBooked(int rideId, int userId)  // passenger Id
        {
            int check = _context.Bookings
                .Where(b => b.RideID == rideId)
                .Where(b => b.BookingStatus == "Pending" || b.BookingStatus == "Accepted")
                .Count(b => b.AppUserID == userId);
            return (check > 0) ? true : false;
        }

        private void SendMessageToDriver(Booking booking)
        {
            Message message = new Message
            {
                SenderId = booking.AppUserID,  // passenger Id
                ReceiverId = booking.Ride.Car.AppUserID,  // driver Id
                SendingDate = DateTime.Now,
                MessageBody =
                $"You have a new booking request from {booking.AppUser.UserName}. You can contact the passenger by using the Reply function."
            };
            _messageService.CreateMessage(message);
        }

        private void SendEmailToDriver(Booking booking)
        {
            string message = $"Dear {booking.Ride.Car.AppUser.FirstName}, You have a new booking by {booking.AppUser.UserName} for the ride on {booking.Ride.StartTime} from {booking.Ride.DepartureLocation} to {booking.Ride.DestinationLocation}. Best regards. Your Zpool-Team";
            _emailSender.SendEmailAsync(booking.Ride.Car.AppUser.Email, "New booking", message);
        }

        

        public Booking GetBookingsByID(int bookingId) 
        {
             return _context.Bookings.Find(bookingId);
        }

        //Method for filtering bookings for users in Bookings page
        public IEnumerable<Booking> GetBookingsByDriversID(AppUser user)
        {
            return _context.Bookings
                .Include(b => b.Ride)
                .ThenInclude(r => r.Car)
                .ThenInclude(c => c.AppUser)
                .Include(b => b.AppUser)
                .Where(b => b.Ride.Car.AppUserID.Equals(user.Id));
        }


        // Method for Profile page
        public IEnumerable<Booking> GetBookingsByUser(AppUser user)
        {
            return from booking
                   in _context.Bookings.Include(r => r.Ride)
                       .ThenInclude(c => c.Car)
                       .ThenInclude(c => c.AppUser)
                       .Where(b => b.AppUserID.Equals(user.Id))
                   select booking;
           
        }

        // Who booked this ride? (bookings)
        public IEnumerable<Booking> GetBookingsByRideId(int rideId)
        {
            return from booking
                   in _context.Bookings.
                   Where(b => b.RideID.Equals(rideId))
                   select booking;
        }

        public void UpdateBookingStatus(int bookingId, string newBookingStatus)
        {           
            Booking oldBooking = _context.Bookings
                .Include(b=>b.Ride)
                .FirstOrDefault(b=>b.BookingID==bookingId);
            
            // ?? has been defined on the UI
            if (oldBooking.BookingStatus == "Cancelled" || oldBooking.BookingStatus == "Rejected")
            {
                throw new ArgumentException("The status of cancelled or rejected bookings cannot be changed.");  // impossible, because no button for changing
            }
            else if ((newBookingStatus == "Rejected" || newBookingStatus == "Accepted")  && oldBooking.BookingStatus != "Pending")
            {
                throw new ArgumentException("Only pending bookings can be changed to accepted, rejected or cancelled.");
            }
            else if (newBookingStatus == "Pending")
            {
                throw new ArgumentException("A booking status cannot be changed to pending.");
            }
            else if(newBookingStatus == "Accepted" && _rideService.SeatsLeft(oldBooking.Ride.RideID) <= 0)
            {
                throw new ArgumentException("The ride is fully booked.");
            }
          
            oldBooking.BookingStatus = newBookingStatus;
            _context.SaveChanges();
        }

        // My Bookings Search function
        public IEnumerable<Booking> GetBookingsByStatus(string status, AppUser user)
        {
            return _context.Bookings
                .Include(b => b.Ride)
                .ThenInclude(r => r.Car)
                .ThenInclude(c => c.AppUser)
                .Include(b => b.AppUser)
                .Where(b=>b.BookingStatus.Equals(status))
                .Where(b=>b.AppUser.Equals(user));
        }

        // top Bookings Search function
        public IEnumerable<Booking> GetBookingsByStatusForDrivers(string status, AppUser user)
        {
            return _context.Bookings
                .Include(b => b.Ride)
                .ThenInclude(r => r.Car)
                .ThenInclude(c => c.AppUser)
                .Include(b => b.AppUser)
                .Where(b => b.BookingStatus.Equals(status))
                .Where(b => b.Ride.Car.AppUser.Equals(user));
        }


        //public void DeleteBooking(Booking booking)
        //{
        //    _context.Bookings.Remove(booking);
        //    _context.SaveChanges();
        //}


        //public void EditBooking(Booking booking)
        //{
        //    _context.Bookings.Update(booking);
        //    _context.SaveChanges();
        //}


        //public IEnumerable<Booking> GetBookings()
        //{
        //    return _context.Bookings
        //        .Include(b => b.Ride).ThenInclude(r => r.Car)
        //        .ThenInclude(c => c.AppUser)
        //        .Include(b => b.AppUser);
        //}


    }
}

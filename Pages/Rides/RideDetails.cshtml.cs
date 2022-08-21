using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.V3.Pages.Internal.Account;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ZPool.Models;
using ZPool.Services.Interfaces;

namespace ZPool.Pages.Rides
{
    [BindProperties]
    public class RideModel : PageModel
    {
        private IRideService _rideService;
        private UserManager<AppUser> _userManager;
        private IBookingService _bookingService;
        private IMessageService _messageService;
        private IReviewService _reviewService;

        public RideModel(IRideService rideService, 
            UserManager<AppUser> userServise, 
            IBookingService bookingService, 
            IMessageService messageService,
            IReviewService reviewService)
        {
            _rideService = rideService;
            _userManager = userServise;
            _bookingService = bookingService;
            _messageService = messageService;
            _reviewService = reviewService;
        }
        
        public Ride Ride { get; set; }
        public int RideId { get; set; }
        public AppUser CurrentUser { get; set; }
        public Message Message { get; set; }
        public int SeatsLeft { get; set; }
        public bool AlreadyBooked { get; set; }
        public CultureInfo culture { get; set; }
        public double UserRating { get; set; }

        public async Task OnGetAsync(int id)
        {
            CurrentUser = await _userManager.GetUserAsync(User);
            Ride = _rideService.GetRide(id);
            RideId = id;
            culture = new CultureInfo("en-US");
            UserRating = _reviewService.GetRatingForUser(Ride.Car.AppUserID);

            if (CurrentUser != null)
            {
                AlreadyBooked = _bookingService.AlreadyBooked(RideId, CurrentUser.Id);
            }
            SeatsLeft = _rideService.SeatsLeft(RideId);
        }

        public async Task<IActionResult> OnPostAsync()
        {
            AppUser user = await _userManager.GetUserAsync(User);
            Ride ride = _rideService.GetRide(RideId);

            if (user != null && ride != null)
            {
                Booking booking = new Booking
                {
                    Date = DateTime.Now,
                    PickUpLocation = ride.DepartureLocation,
                    DropOffLocation = ride.DestinationLocation,
                    RideID = ride.RideID,
                    AppUserID = user.Id,
                    BookingStatus = "Pending"
                };
                _bookingService.AddBooking(booking);   // Book the ride
            }
            
            return RedirectToPage("/Account/Manage/MyBookings", new { area = "Identity" });
        }

        public IActionResult OnPostDelete(int rideId)
        {
            Ride ride = _rideService.GetRide(rideId);
            _rideService.DeleteRide(ride);
            return RedirectToPage("/Rides/GetRides");
        }

        public async Task<IActionResult> OnPostSend()
        {
            CurrentUser = await _userManager.GetUserAsync(User);
            Message.SendingDate = DateTime.Now;
            _messageService.CreateMessage(Message);
            return RedirectToPage("/Rides/RideDetails", new { id = RideId});
        }
    }
}

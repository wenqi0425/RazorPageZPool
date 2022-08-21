using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ZPool.Models;
using ZPool.Services.Interfaces;

namespace ZPool.Areas.Identity.Pages.Account.Manage
{
    public class WhoBookTheRideModel : PageModel
    {
        public IEnumerable<Booking> BookingsOfOneRide { get; set; }

        public UserManager<AppUser> _maneger;
        public IBookingService _bookingService;
        public IRideService _rideService;

        public Ride MyRide { get; set; }
        public string Message { get; set; }


        // DI is used to initialize the instance of PageModel class
        public WhoBookTheRideModel(UserManager<AppUser> maneger, IBookingService bookingService, IRideService rideService)
        {
            _maneger = maneger;
            _bookingService = bookingService;
            _rideService = rideService;
        }

        public async Task OnGetAsync(int id)  // ride id
        {
            AppUser user = await _maneger.GetUserAsync(User);
            MyRide = _rideService.GetRide(id);

            await LoadBookingByRideId(id);
        }

        private async Task LoadBookingByRideId(int rideId)
        {
            List<Booking> bookings = _bookingService.GetBookingsByRideId(rideId).ToList();

            //loading dependent AppUser for each Booking
            foreach (Booking booking in bookings)
            {
                int appUserId = booking.AppUserID;

                /* It is similar as Lazy loading, to improve the performance.          
                    booking table has a Navigation property AppUser.
                    1) Eager loading: to load all the Navigation properties of Booking table. 
                       We need to communicate the DB several times with bad performance. 
                    2) Lazy loading: only load the bookings or the Navigation property when we need it. 
                        2.1£©first step: only get the userId from booking table, not the whole table.
                        2.2£©second step: find the user by userId.
                 */

                await _maneger.FindByIdAsync(appUserId.ToString());  
            }

            BookingsOfOneRide = bookings;  // BookingsOfOneRide£º booking + passenger
        }

        private int GetRideIdFromBooking(int bookingId)
        {
            Booking currentBooking = _bookingService.GetBookingsByID(bookingId);
            return currentBooking.RideID;
        }

        public async Task OnPostAccept(int id)
        {
            try
            {
                _bookingService.UpdateBookingStatus(id, "Accepted");
            }
            catch (Exception ex)
            {
                Message = ex.Message;
            }

            // reconstruct the data senario
            // after Accept, refresh page£¬rideId lost, but still has bookingId, to get rideId again. 
            int rideId = GetRideIdFromBooking(id); 
            await LoadBookingByRideId(rideId);
            MyRide = _rideService.GetRide(rideId);

            RedirectToPage("WhoBookTheRide");
        }

        public async Task OnPostReject(int id)
        {
            try
            {
                _bookingService.UpdateBookingStatus(id, "Rejected");
            }
            catch (Exception ex)
            {
                Message = ex.Message;
            }

            //reconstruc the data senario            
            int rideId = GetRideIdFromBooking(id);
            await LoadBookingByRideId(rideId);
            MyRide = _rideService.GetRide(rideId);

            RedirectToPage("WhoBookTheRide");
        }

        public async Task OnPostCancel(int id)
        {
            try
            {
                _bookingService.UpdateBookingStatus(id, "Cancelled");
            }
            catch (Exception ex)
            {
                Message = ex.Message;
            }

            //reconstruc the data senario            
            int rideId = GetRideIdFromBooking(id);
            await LoadBookingByRideId(rideId);
            MyRide = _rideService.GetRide(rideId);

            RedirectToPage("WhoBookTheRide");
        }
    }
}

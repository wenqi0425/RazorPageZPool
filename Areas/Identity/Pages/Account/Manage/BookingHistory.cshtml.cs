using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using UserManagementTestApp.Models;
using ZPool.Models;
using ZPool.Services.Interface;

namespace ZPool.Areas.Identity.Pages.Account.Manage
{
    public class BookingHistoryModel : PageModel
    {
        public UserManager<AppUser> _manager;
        public IBookingService _bookingService;
        public List<Booking> MyHistoricBookings { get; set; }
        public Booking OneHistoricBooking { get; set; }

        public BookingHistoryModel(IBookingService service, UserManager<AppUser> manager)
        {
            _bookingService = service;
            _manager = manager;

            MyHistoricBookings = new List<Booking>();
        }

        public void OnGet(int bookingId)
        {
            //AppUser user = await _manager.GetUserAsync(User);
            var history = _bookingService.GetBookingsByID(bookingId);

            if (history.BookingStatus.Equals("Cancelled") || history.BookingStatus.Equals("Rejected")) 
            {
                OneHistoricBooking = _bookingService.GetBookingsByID(bookingId);
                MyHistoricBookings.Add(OneHistoricBooking);
            } 
        }

        public async Task OnPost(int id)
        {
            AppUser user = await _manager.GetUserAsync(User);
            MyHistoricBookings = _bookingService.GetBookingsByUser(user);

            RedirectToPage("MyBookings");
        }


        //public void OnGet()
        //{
        //}
    }
}

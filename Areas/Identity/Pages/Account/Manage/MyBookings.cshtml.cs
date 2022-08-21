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
    public class MyBookingsModel : PageModel
    {
        public UserManager<AppUser> Manager;
        public IBookingService BookingService;

        public IEnumerable<Booking> MyBookings { get; set; }
        public string Message { get; set; }

        [BindProperty] public string StatusCriteria { get; set; }

        public MyBookingsModel(IBookingService service, UserManager<AppUser> manager)
        {
            BookingService = service;
            Manager = manager;
        }

        public async Task OnGet()
        {
            AppUser user = await Manager.GetUserAsync(User);
            MyBookings = BookingService.GetBookingsByUser(user).OrderByDescending(b=>b.Date);
        }

        public async Task OnPostCancel(int id)
        {
            try
            {
                BookingService.UpdateBookingStatus(id, "Cancelled");
            }
            catch (Exception ex)
            {
                Message = ex.Message;
            }
            AppUser user = await Manager.GetUserAsync(User);
            MyBookings = BookingService.GetBookingsByUser(user);

            RedirectToPage("MyBookings");

            //RedirectToPage("GetBookings");
        }

        public async Task OnPostStatusFilter(string status)
        {
            if (!String.IsNullOrEmpty(StatusCriteria))
            {
                AppUser user = await Manager.GetUserAsync(User);
                MyBookings = BookingService.GetBookingsByStatus(StatusCriteria, user);
            }
            
            RedirectToPage("MyBookings");
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ZPool.Models;
using ZPool.Services.Interfaces;

namespace ZPool.Pages.Bookings
{
    [Authorize] 
    public class GetBookingsModel : PageModel
    {
        IBookingService bookingService;
        UserManager<AppUser> userManager;
        public GetBookingsModel(IBookingService service, UserManager<AppUser> manager)
        {
            bookingService = service;
            userManager = manager;
        }

        public IEnumerable<Booking> Bookings { get; set; }
        [BindProperty] public string StatusCriteria { get; set; }
        [BindProperty] public AppUser LoggedInUser { get; set; }

        public string Message { get; set; }

        
        public async Task OnGet()
        {
            LoggedInUser = await userManager.GetUserAsync(User);
            Bookings = bookingService
                .GetBookingsByDriversID(LoggedInUser)
                .OrderByDescending(b => b.Date);
        }

        public async Task OnPostAccept(int id)
        {
            LoggedInUser = await userManager.GetUserAsync(User);
            Bookings = bookingService.GetBookingsByDriversID(LoggedInUser);

            try
            {
                // Exceptions and SaveChange are defined on EFBookingService
                bookingService.UpdateBookingStatus(id, "Accepted");
            }
            catch (Exception ex)
            {
                Message = ex.Message;  
            }
        }

        public async Task OnPostReject(int id)
        {
            LoggedInUser = await userManager.GetUserAsync(User);
            Bookings = bookingService.GetBookingsByDriversID(LoggedInUser);

            try
            {
                bookingService.UpdateBookingStatus(id, "Rejected");
            }
            catch (Exception ex)
            {
                Message = ex.Message;
            }
        }

        public async Task OnPostCancel(int id)
        {
            LoggedInUser = await userManager.GetUserAsync(User);
            Bookings = bookingService.GetBookingsByDriversID(LoggedInUser);

            try
            {
                bookingService.UpdateBookingStatus(id, "Cancelled");
            }
            catch (Exception ex)
            {
                Message = ex.Message;
            }   
        }

        public async Task<IActionResult> OnPostStatusFilter()
        {
            LoggedInUser = await userManager.GetUserAsync(User);
            if (!String.IsNullOrEmpty(StatusCriteria))
            {
                //AppUser user = await userManager.GetUserAsync(User);
                Bookings = bookingService.GetBookingsByStatusForDrivers(StatusCriteria, LoggedInUser);
            }

            return Page();
        }
    }
}

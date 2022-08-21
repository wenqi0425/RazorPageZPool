using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ZPool.Models;
using ZPool.Services.Interfaces;

namespace ZPool.Pages.Bookings
{
    public class EditModel : PageModel
    {
        [BindProperty]
        public Booking booking { get; set; }
        IBookingService bookingservice;
        public EditModel(IBookingService service)
        {
            bookingservice = service;
        }

        public IActionResult OnGet(int id)
        {
            booking = bookingservice.GetBookingsByID(id);
            if (booking == null)
            {
                return null;
            }
            return Page();
        }

        //public IActionResult OnPost()
        //{
        //    if (!ModelState.IsValid)
        //    {
        //        return Page();
        //    }
        //    bookingservice.EditBooking(booking);
        //    return RedirectToPage();
        //}
    }
}


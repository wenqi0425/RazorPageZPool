using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using ZPool.Models;
using ZPool.Services.Interfaces;

namespace ZPool.Pages.Bookings
{
    public class DeleteModel : PageModel
    {
        [BindProperty]
        public Booking Booking { get; set; }
        IBookingService bookingservice;
        public DeleteModel(IBookingService service)
        {
            bookingservice = service;
        }

        public IActionResult OnGet(int id)
        {
            Booking = bookingservice.GetBookingsByID(id);
            return Page();
        }

        //public IActionResult OnPost(Booking booking)
        //{
           
        //        bookingservice.DeleteBooking(booking);
            


        //    return RedirectToPage();
        //}
    }
}










        //    private readonly ZPool.Models.AppDbContext _context;

        //    public DeleteModel(ZPool.Models.AppDbContext context)
        //    {
        //        _context = context;
        //    }

        //    [BindProperty]
        //    public Booking Booking { get; set; }

        //    public async Task<IActionResult> OnGetAsync(int? id)
        //    {
        //        if (id == null)
        //        {
        //            return NotFound();
        //        }

        //        Booking = await _context.Bookings
        //            .Include(b => b.AppUser)
        //            .Include(b => b.Ride).FirstOrDefaultAsync(m => m.BookingID == id);

        //        if (Booking == null)
        //        {
        //            return NotFound();
        //        }
        //        return Page();
        //    }

        //    public async Task<IActionResult> OnPostAsync(int? id)
        //    {
        //        if (id == null)
        //        {
        //            return NotFound();
        //        }

        //        Booking = await _context.Bookings.FindAsync(id);

        //        if (Booking != null)
        //        {
        //            _context.Bookings.Remove(Booking);
        //            await _context.SaveChangesAsync();
        //        }

        //        return RedirectToPage("./Index");
        //    }
        //}
    

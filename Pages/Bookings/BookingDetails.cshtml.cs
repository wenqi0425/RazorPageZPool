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
    public class DetailsModel : PageModel
    {
        private readonly AppDbContext _context;


        public DetailsModel(AppDbContext context)
        {
            _context = context;
        }

        public Booking Booking { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Booking = await _context.Bookings
                .Include(b => b.AppUser)
                .Include(b => b.Ride).FirstOrDefaultAsync(m => m.BookingID == id);

            if (Booking == null)
            {
                return NotFound();
            }
            return Page();
        }
    }
}

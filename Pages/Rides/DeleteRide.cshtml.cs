using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ZPool.Models;
using ZPool.Services.Interfaces;

namespace ZPool.Pages.Rides
{
    [Authorize]
    public class DeleteRideModel : PageModel
    {
        IRideService rideService;

        [BindProperty]
        public Ride Ride { get; set; }

        public DeleteRideModel (IRideService service)
        {
            rideService = service;
        }
        public IActionResult OnGet(int id)
        {
            Ride = rideService.GetRide(id);
            return Page();
        }

        public IActionResult OnPost(Ride ride)
        {
            rideService.DeleteRide(ride);
            return RedirectToPage("GetRides");
        }
    }
}

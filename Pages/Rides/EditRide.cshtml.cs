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
    public class EditRideModel : PageModel
    {
        IRideService rideService;

        [BindProperty]
        public Ride ride { get; set; }

        public EditRideModel(IRideService service)
        {
            rideService = service;
        }

        public IActionResult OnGet(int id)
        {
            ride = rideService.GetRide(id);
            if (ride == null)
            {
                return null;
            }
            return Page();
        }

        public IActionResult OnPost()
        {
            if(!ModelState.IsValid)
            {
                return Page();
            }
            rideService.EditRide(ride);
            return RedirectToPage("GetRides");
        }
       
    }
}

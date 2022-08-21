using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ZPool.Models;
using ZPool.Services.Interfaces;

namespace ZPool.Pages.Rides
{
    public class GetAllRidesModel : PageModel
    {
        IRideService rideService;

        public GetAllRidesModel(IRideService service)
        {
            rideService = service;
        }

        public IEnumerable<Ride> Rides { get; set; }
        public AppUser CurrentUser { get; set; }
        [BindProperty(SupportsGet = true)]
        public RideCriteriaInputModel RideCriteria { get; set; } = new RideCriteriaInputModel();
        public string ScreenMessage { get; set; }

        public void OnGet()
        {
            if (!string.IsNullOrEmpty(RideCriteria.DepartureLocation) 
                || !string.IsNullOrEmpty(RideCriteria.DestinationLocation))
            {
                Rides = rideService.FilterRides(RideCriteria);
                if (Rides.Count() == 0)
                {
                    ScreenMessage = "Sorry! We couldn't match any rides to your request.";
                }
            }
            else
            {
                Rides = rideService.GetAllRides().OrderBy(r=>r.StartTime);
                RideCriteria.StartTime = DateTime.Now;
            }
        }
    }

    public class RideCriteriaInputModel
    {
        public string DepartureLocation { get; set; }
        public string DestinationLocation { get; set; }
        public DateTime StartTime { get; set; }
    }
}

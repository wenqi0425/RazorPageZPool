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
    public class DeleteMyRideModel : PageModel
    {
        [BindProperty]
        public Ride MyRide { get; set; }

        public IRideService _service;

        public DeleteMyRideModel(IRideService service)
        {
            _service = service;
            MyRide = new Ride();
        }
        public void OnGet(int id)
        {
            MyRide = _service.GetRide(id);
        }

        public IActionResult OnPost()
        {
            _service.DeleteRide(MyRide);

            return RedirectToPage("./MyRides");
        }



    }
}

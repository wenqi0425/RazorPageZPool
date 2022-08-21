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
    public class CreateCarModel : PageModel
    {
        [BindProperty]
        public Car Car { get; set; }
        public string Message { get; set; }
        ICarService carService;
        private UserManager<AppUser> _userManager;
    

    

        public CreateCarModel(ICarService service, UserManager<AppUser> userManager)
        {
            this.carService = service;
            _userManager = userManager;
        }
        public void OnGet()
        {

        }

        public async Task<IActionResult> OnPostAsync(Car car)
        {
            AppUser user = await _userManager.GetUserAsync(User);
            
            if (!ModelState.IsValid)
            {
                return Page();  // nothing change
            }

            string typedNumberPlate = car.NumberPlate;
            bool Carfound;
            Carfound = false;  // default
            IEnumerable<Car> carPlates = carService.GetCarsNumberPlate(typedNumberPlate);
            
            if ( carPlates.Count() > 0)   
            {
                Carfound = true;    // 1)
            }

            if (Carfound == false)
            {
                car.AppUserID = user.Id;
                carService.AddCar(car);   // 2)
                return RedirectToPage("MyCars");
            }
            else  // Carfound == true
            {
                Message = "You cannot add two cars with the same number plate";  // 3)
                return Page();
            }
        }
    }
}

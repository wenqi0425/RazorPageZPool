using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ZPool.Models;
using ZPool.Services.Interfaces;

namespace ZPool.Areas.Identity.Pages.Account.Manage
{
    public class DeleteCarModel : PageModel
    {
        [BindProperty]
        public Car Car { get; set; }

        ICarService carService;

        public DeleteCarModel(ICarService service)
        {
            this.carService = service;
            
        }
        public void OnGet(int id)
        {
            Car = carService.GetCar(id);
        }
        public IActionResult OnPost()
        {
            carService.DeleteCar(Car);

            return RedirectToPage("./MyCars");
        }
    }
}

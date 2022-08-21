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
    public class MyCarsModel : PageModel
    {
        public UserManager<AppUser> _userManager;
        public IEnumerable<Car> Cars { get; set; } = new List<Car>();
        public Car Car { get; set; }

        private ICarService context;
        public MyCarsModel(ICarService service, UserManager<AppUser> userManager)
        {
            context = service;
            _userManager = userManager;
        }
        public async Task OnGet()
        {
            AppUser user = await _userManager.GetUserAsync(User);
            Cars = context.GetCarsByUser(user.Id);
        }
    }
}

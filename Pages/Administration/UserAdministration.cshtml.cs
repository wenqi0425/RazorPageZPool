using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Sentry.Protocol;
using ZPool.Models;

namespace ZPool.Pages.Administration
{
    [Authorize(Roles = "Admin")]   // Microsoft.AspNetCore.Authorization.Roles
    public class UserAdministrationModel : PageModel
    {
        private UserManager<AppUser> _userManager;

        public UserAdministrationModel(UserManager<AppUser> userManager)
        {
            _userManager = userManager;
        }

        public IEnumerable<AppUser> Users { get; set; }

        public string StatusMessage { get; set; }
        [BindProperty(SupportsGet = true)]
        public string SearchCriteria { get; set; }

        public async Task<IActionResult> OnGetAsync(string statusMessage)
        {
            if (!string.IsNullOrEmpty(SearchCriteria))
            {
                AppUser user = await _userManager.FindByEmailAsync(SearchCriteria);
                if (user != null)
                {
                    List<AppUser> users = new List<AppUser>();
                    users.Add(user);
                    Users = users;
                }
                else
                {
                    return RedirectToPage(new {statusMessage = "Error: No user found."});
                }
            }
            else
            {
                StatusMessage = statusMessage;
                Users = _userManager.Users;
                
            }
            return Page();
        }
    }
}

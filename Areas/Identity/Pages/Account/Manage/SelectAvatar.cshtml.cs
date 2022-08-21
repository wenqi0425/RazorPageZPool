using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using ZPool.Models;
using ZPool.Services.Interfaces;

namespace ZPool.Areas.Identity.Pages.Account.Manage
{
    public class SelectAvatarModel : PageModel
    {
        private readonly UserManager<AppUser> _manager;

        public SelectList AvatarList { get; set; }
        public AppUser LoggedInUser { get; set; }
        [BindProperty]public string UserAvatarName { get; set; }

        public SelectAvatarModel(UserManager<AppUser> manager)
        {
            _manager = manager;
        }

        public async Task OnGet()
        {
            AppUser user = await _manager.GetUserAsync(User);

            await LoadAsync(user);
        }

        private async Task LoadAsync(AppUser user)
        {
            LoggedInUser = await _manager.GetUserAsync(User);
            UserAvatarName = user.AvatarName;

            if (string.IsNullOrEmpty(UserAvatarName))
            {
                UserAvatarName = "default.png";
            }
            else
            {
                UserAvatarName = user.AvatarName;
            }
        }

        public async Task<IActionResult> OnPostAsync()
        {
            var user = await _manager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{_manager.GetUserId(User)}'.");
            }

            if (!ModelState.IsValid)
            {
                await LoadAsync(user);
                return Page();
            }      

            var avatar = user.AvatarName;
            if (UserAvatarName != avatar)
            {
                user.AvatarName = UserAvatarName;
                await _manager.UpdateAsync(user);
            }
            
            return RedirectToPage("Index");
        }
    }
}

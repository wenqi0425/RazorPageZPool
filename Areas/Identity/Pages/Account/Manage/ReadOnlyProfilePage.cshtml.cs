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
    public class ReadOnlyProfilePageModel : PageModel
    {
        private readonly UserManager<AppUser> _userManager;
        private IReviewService _reviewService;

        public ReadOnlyProfilePageModel(UserManager<AppUser> userManager, IReviewService reviewService)
        {
            _userManager = userManager;
            _reviewService = reviewService;
        }

        // UserToCheck: the person who is checked by others
        public AppUser UserToCheck { get; set; }  
        public string UserName { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Introduction { get; set; }
        public string UserAvatarName { get; set; }
        public string UserGender { get; set; }
        public double UserRating { get; set; }

        public async Task<IActionResult> OnGetAsync(int id)
        {
            var user = await _userManager.FindByIdAsync(id.ToString());

            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            await LoadAsync(user);
            return Page();
        }

        private async Task LoadAsync(AppUser user)
        {
            //UserToCheck = await _userManager.GetUserAsync(User);
            UserToCheck = user;

            UserName = user.UserName;
            FirstName = user.FirstName;
            LastName = user.LastName;
            UserGender = user.Gender;
            Introduction = user.Introduction;
            UserRating = _reviewService.GetRatingForUser(user.Id);

            string avatarName = user.AvatarName;
            if (avatarName == "" || avatarName == null)
            {
                UserAvatarName = "default.png";
            }
            else
            {
                UserAvatarName = avatarName;
            }
        }

        public async Task<IActionResult> OnPostAsync()
        {
            var user = await _userManager.GetUserAsync(User);        

            user.FirstName = FirstName;
            user.LastName = LastName;
            user.Gender = UserGender;
            user.Introduction = Introduction;
            user.AvatarName = UserAvatarName;

            return RedirectToPage();
        }
    }
}


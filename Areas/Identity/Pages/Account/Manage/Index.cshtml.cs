using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using ZPool.Models;
using ZPool.Services.Interfaces;

namespace ZPool.Areas.Identity.Pages.Account.Manage
{
    public partial class IndexModel : PageModel
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        private IReviewService _reviewService;


        public IndexModel(
            UserManager<AppUser> userManager, 
            SignInManager<AppUser> signInManager, IReviewService reviewService)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _reviewService = reviewService;
        }

        [TempData] public string StatusMessage { get; set; }

        [BindProperty] public string FirstName { get; set; }
        [BindProperty] public string LastName { get; set; }     
        [BindProperty] public string Introduction { get; set; }  // It is "About Me" 
        [BindProperty] public string UserAvatarName { get; set; }
        [BindProperty] public string UserGender { get; set; }

        public SelectList GenderList { get; set; }
        public AppUser LoggedInUser { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public double UserRating { get; set; }

        [BindProperty] public InputModel Input { get; set; }

        public class InputModel
        {
            public string UserGender { get; set; }
            public string Introduction { get; set; }
            public string FirstName { get; set; }
            public string LastName { get; set; }
        }

        public async Task<IActionResult> OnGetAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            await LoadAsync(user);
            return Page();
        }

        private async Task LoadAsync(AppUser user)
        {
            LoggedInUser = await _userManager.GetUserAsync(User);

            Username = user.UserName;
            FirstName = user.FirstName;
            LastName = user.LastName;
            Email = user.Email;
            UserGender = user.Gender;
            Introduction = user.Introduction;
            UserAvatarName = user.AvatarName;
            UserRating = _reviewService.GetRatingForUser(user.Id);

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
            var user = await _userManager.GetUserAsync(User);        

            user.FirstName = FirstName;
            user.LastName = LastName;
            user.Gender = UserGender;
            user.Introduction = Introduction;

            await _userManager.UpdateAsync(user);
            await _signInManager.RefreshSignInAsync(user);
            StatusMessage = "Your profile has been updated";
            return RedirectToPage();
        }
    }
}

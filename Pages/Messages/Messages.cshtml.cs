using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ZPool.Models;
using ZPool.Services.Interfaces;

namespace ZPool.Pages.Messages
{
    // Fan and Chasm trap between message and booking 

    [Authorize]
    public class MessagesModel : PageModel
    {
        private IMessageService _messageService;
        private UserManager<AppUser> _userManager;

        public MessagesModel(IMessageService messageService, UserManager<AppUser> userManager)
        {
            _messageService = messageService;
            _userManager = userManager;
        }

        [BindProperty] public Message NewMessage { get; set; }
        [ValidateNever] public AppUser CurrentUser { get; set; } 
        [BindProperty] public int ListLength { get; set; }
        
        public List<Message> Messages { get; set; }

        public async Task<IActionResult> OnGetAsync()
        {
            CurrentUser = await _userManager.GetUserAsync(User);

            if (CurrentUser == null)  
            {
                return RedirectToPage("/Account/Login", 
                    new { area = "Identity", returnUrl = "/Pages/Messages/Messages"});
            }

            Messages = _messageService.GetMessagesByUserId(CurrentUser.Id);
            ListLength = 8;  
            return Page();
        }

        public IActionResult OnPostSend()
        {
            NewMessage.SendingDate = DateTime.Now;
            if (!ModelState.IsValid)
            {
                return RedirectToPage("Messages");
            }
            _messageService.CreateMessage(NewMessage);
            return RedirectToPage("Messages");
        }

        public async Task<IActionResult> OnPostReplyAsync()
        {
            CurrentUser = await _userManager.GetUserAsync(User);
            // before reply: /Messages/Messages£¬ after: Messages/Messages?handler=Reply
            Messages = _messageService.GetMessagesByUserId(CurrentUser.Id);
            return Page();
        }

        public async Task<IActionResult> OnPostLoadAsync()
        {
            CurrentUser = await _userManager.GetUserAsync(User);
            Messages = _messageService.GetMessagesByUserId(CurrentUser.Id);
            ListLength += 5;
            return Page();
        }
    }
}

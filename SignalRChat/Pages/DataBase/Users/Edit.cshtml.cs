using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using SignalRChat.Data;
using SignalRChat.Models;

namespace SignalRChat.Pages.DataBase.Users
{
    public class EditUserModel : PageModel
    {
        private readonly SignalRChat.Data.SignalRChatContextIdentity _context;
        UserManager<IdentityUser> _userManager;
        private readonly ILogger<EditUserModel> _logger;

        public EditUserModel(SignalRChat.Data.SignalRChatContextIdentity context, UserManager<IdentityUser> userManager, ILogger<EditUserModel> logger)
        {
            _context = context;
            _userManager = userManager;
            _logger = logger;
        }
        [BindProperty]
        public IdentityUser getUser { get; set; }

        public async Task<IActionResult> OnGetAsync(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            getUser = await _context.Users.FirstOrDefaultAsync(m => m.Id == id);

            if (getUser == null)
            {
                return NotFound();
            }
            return Page();
        }

        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync(string id)
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            var User = await _userManager.FindByIdAsync(id);
            if (User != null)
            {
                User.UserName = getUser.UserName;
                User.Email = getUser.Email;
                User.PasswordHash = getUser.PasswordHash;
                var result = await _userManager.UpdateAsync(User);
                if (result.Succeeded)
                {
                    return RedirectToPage("./Index");
                }
                else
                {
                    foreach (var error in result.Errors)
                    {
                        _logger.LogError("Ошибка при изменении записи: {0}", error);
                    }
                }
            }
            return Page();
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using SignalRChat.Data;
using SignalRChat.Models;

namespace SignalRChat.Pages.DataBase.Users
{
    public class DeleteUserModel : PageModel
    {
        private readonly SignalRChat.Data.SignalRChatContextIdentity _context;
        UserManager<IdentityUser> _userManager;

        public DeleteUserModel(SignalRChat.Data.SignalRChatContextIdentity context, UserManager<IdentityUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

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

        public async Task<IActionResult> OnPostAsync(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var User = await _userManager.FindByIdAsync(id);

            if (User != null)
            {
                IdentityResult result = await _userManager.DeleteAsync(User);
            }

            return RedirectToPage("./Index");
        }
    }
}

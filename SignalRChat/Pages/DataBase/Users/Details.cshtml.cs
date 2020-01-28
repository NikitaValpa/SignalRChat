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
    public class DetailsUserModel : PageModel
    {
        private readonly SignalRChat.Data.SignalRChatContextIdentity _context;

        public DetailsUserModel(SignalRChat.Data.SignalRChatContextIdentity context)
        {
            _context = context;
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
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Identity;

namespace SignalRChat.Pages.DataBase.Roles
{
    public class DetailsRoleModel : PageModel
    {
        private readonly Data.SignalRChatContextIdentity _context;
        private RoleManager<IdentityRole> _RoleManager;

        public DetailsRoleModel(Data.SignalRChatContextIdentity context, RoleManager<IdentityRole> roleManager)
        {
            _context = context;
            _RoleManager = roleManager;
        }

        public IdentityRole getRole { get; set; }
        public async Task<IActionResult> OnGetAsync(string id)
        {
            if (id == null)
            {
                return NotFound();
            }
            getRole = await _context.Roles.FindAsync(id);
            if (getRole == null)
            {
                return NotFound();
            }
            return Page();
        }
    }
}
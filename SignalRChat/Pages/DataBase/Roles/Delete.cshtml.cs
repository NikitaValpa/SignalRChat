using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Identity;

namespace SignalRChat.Pages.DataBase.Roles
{
    public class DeleteRoleModel : PageModel
    {
        private readonly SignalRChat.Data.SignalRChatContextIdentity _context;
        RoleManager<IdentityRole> _roleManager;

        public DeleteRoleModel(SignalRChat.Data.SignalRChatContextIdentity context, RoleManager<IdentityRole> roleManager)
        {
            _context = context;
            _roleManager = roleManager;
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

        public async Task<IActionResult> OnPostAsync(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var Role = await _roleManager.FindByIdAsync(id);

            if (Role != null)
            {
                IdentityResult result = await _roleManager.DeleteAsync(Role);
            }

            return RedirectToPage("./Index");
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;

namespace SignalRChat.Pages.DataBase.Roles
{
    public class CreateRoleModel : PageModel
    {
        private readonly Data.SignalRChatContextIdentity _context;
        RoleManager<IdentityRole> _roleManager;
        private readonly ILogger<EditRoleModel> _logger;

        public CreateRoleModel(Data.SignalRChatContextIdentity context, RoleManager<IdentityRole> roleManager, ILogger<EditRoleModel> logger)
        {
            _context = context;
            _roleManager = roleManager;
            _logger = logger;
        }
        [BindProperty]
        public IdentityRole getRole { get; set; }

        public IActionResult OnGet()
        {
            return Page();
        }

        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            var User = new IdentityRole { Name = getRole.Name, NormalizedName = getRole.NormalizedName };
                
            var result = await _roleManager.CreateAsync(User);
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
            return Page();
        }
    }
}
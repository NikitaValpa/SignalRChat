using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;

namespace SignalRChat.Pages.DataBase.Users
{
    public class EditRolesUserModel : PageModel
    {
        private readonly SignalRChat.Data.SignalRChatContextIdentity _context;
        UserManager<IdentityUser> _userManager;
        RoleManager<IdentityRole> _roleManager;
        private readonly ILogger<EditUserModel> _logger;

        public EditRolesUserModel(SignalRChat.Data.SignalRChatContextIdentity context, UserManager<IdentityUser> userManager, ILogger<EditUserModel> logger, RoleManager<IdentityRole> roleManager)
        {
            _context = context;
            _userManager = userManager;
            _logger = logger;
            _roleManager = roleManager;
        }
        [BindProperty]
        public IdentityUser getUser { get; set; }
        [BindProperty]
        public List<IdentityRole> AllRoles { get; set; }

        public IList<string> UserRoles { get; set; }
        public async Task<IActionResult> OnGetAsync(string id)
        {
            getUser = await _userManager.FindByIdAsync(id);
            if (getUser == null)
            {
                return NotFound();
            }
            UserRoles = await _userManager.GetRolesAsync(getUser);
            AllRoles = _roleManager.Roles.ToList();
            return Page();
        }
        public async Task<IActionResult> OnPostAsync(string userId, List<string> roles)
        {
            var User = await _userManager.FindByIdAsync(userId);
            var UserRoles = await _userManager.GetRolesAsync(User);
            if (UserRoles != null)
            {
                var addedRoles = roles.Except(UserRoles);//Получаем список ролей которые нужно добавить   
                await _userManager.AddToRolesAsync(User, addedRoles); // Добавляем юзеру те роли которые нужно добавить

                var removedRoles = UserRoles.Except(roles);// Получаем список ролей которые нужно убрать
                await _userManager.RemoveFromRolesAsync(User, removedRoles);// Убераем у юзера те роли, которые нужно убрать
            }
            else
            {
                await _userManager.AddToRolesAsync(User, roles); // Добавляем юзеру отмеченные роли
            }
            return RedirectToPage("./Index");
        }
    }
}
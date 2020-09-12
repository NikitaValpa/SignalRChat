using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Logging;
using SignalRChat.Data;

namespace SignalRChat.Pages
{
    public class ChangePasswordModel : PageModel
    {
        private readonly ILogger<ChatModel> _logger;
        private readonly UserManager<IdentityUser> _userManager;
        public ChangePasswordModel(ILogger<ChatModel> logger, UserManager<IdentityUser> userManager) {
            _logger = logger;
            _userManager = userManager;
        }

        public string Id { get; set; }
        public string Email { get; set; }
        public string NewPassword { get; set; }

        public async Task<IActionResult> OnGetAsync(string id)
        {
            IdentityUser user = await _userManager.FindByIdAsync(id);
            if (user == null)
            {
                return NotFound();
            }
            //ChangePasswordViewModel model = new ChangePasswordViewModel { Id = user.Id, Email = user.Email };
            Id = user.Id;
            Email = user.Email;
            
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(string Id, string Email, string NewPassword) {

            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByIdAsync(Id);
                if (user != null)
                {
                    var _passwordValidator =
                        HttpContext.RequestServices.GetService(typeof(IPasswordValidator<IdentityUser>)) as IPasswordValidator<IdentityUser>;
                    var _passwordHasher =
                        HttpContext.RequestServices.GetService(typeof(IPasswordHasher<IdentityUser>)) as IPasswordHasher<IdentityUser>;

                    IdentityResult result =
                        await _passwordValidator.ValidateAsync(_userManager, user, NewPassword);
                    if (result.Succeeded)
                    {
                        user.PasswordHash = _passwordHasher.HashPassword(user, NewPassword);
                        await _userManager.UpdateAsync(user);
                        return RedirectToPage("/DataBase/Users/Index");
                    }
                    else
                    {
                        foreach (var error in result.Errors)
                        {
                            ModelState.AddModelError(string.Empty, error.Description);
                        }
                    }
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Пользователь не найден");
                }
            }

            return Page();
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Identity;

namespace SignalRChat.Pages.DataBase.Roles
{
    public class RolesModel : PageModel
    {
        Data.SignalRChatContextIdentity _context;
        public List<IdentityRole> Roles { get; set; }
        public RolesModel(Data.SignalRChatContextIdentity context)
        {
            _context = context;
        }
        public void OnGet()
        {
            Roles = _context.Roles.ToList();
        }
    }
}
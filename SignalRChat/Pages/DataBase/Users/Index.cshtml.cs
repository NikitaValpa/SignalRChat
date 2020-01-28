using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Identity;

namespace SignalRChat.Pages.DataBase.Users
{
    public class UsersModel : PageModel
    {
        Data.SignalRChatContextIdentity _context;
        public List<IdentityUser> Users { get; set; }
        public UsersModel(Data.SignalRChatContextIdentity context)
        {
            _context = context;
        }
        public void OnGet()
        {
            Users = _context.Users.ToList();
        }
    }
}
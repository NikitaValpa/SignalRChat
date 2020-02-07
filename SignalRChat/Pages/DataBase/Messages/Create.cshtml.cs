using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using SignalRChat.Data;
using SignalRChat.Models;

namespace SignalRChat.Pages.DataBase.Messages
{
    public class CreateMessageModel : PageModel
    {
        private readonly SignalRChat.Data.SignalRChatContext _context;

        public CreateMessageModel(SignalRChat.Data.SignalRChatContext context)
        {
            _context = context;
        }

        public IActionResult OnGet()
        {
            return Page();
        }

        [BindProperty]
        public Models.Messages Messages { get; set; }

        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            _context.Messages.Add(new Models.Messages {Name = Messages.Name, Message = Messages.Message, SendDate = DateTime.Now});
            await _context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }
    }
}

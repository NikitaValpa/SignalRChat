using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SignalRChat.Data;
using SignalRChat.Models;

namespace SignalRChat.Pages.DataBase.Messages
{
    public class EditMessageModel : PageModel
    {
        private readonly SignalRChat.Data.SignalRChatContext _context;

        public EditMessageModel(SignalRChat.Data.SignalRChatContext context)
        {
            _context = context;
        }

        [BindProperty]
        public Models.Messages Messages { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Messages = await _context.Messages.FirstOrDefaultAsync(m => m.ID == id);

            if (Messages == null)
            {
                return NotFound();
            }
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

            _context.Attach(Messages).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!MessagesExists(Messages.ID))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return RedirectToPage("./Index");
        }

        private bool MessagesExists(int id)
        {
            return _context.Messages.Any(e => e.ID == id);
        }
    }
}

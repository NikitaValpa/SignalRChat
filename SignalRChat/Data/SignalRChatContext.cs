using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using SignalRChat.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace SignalRChat.Data
{
    public class SignalRChatContext : DbContext
    {
        public SignalRChatContext (DbContextOptions<SignalRChatContext> options) : base(options)
        {
            Database.EnsureCreated();
        }

        public DbSet<Models.Messages> Messages { get; set; }
    }
    public class SignalRChatContextIdentity : IdentityDbContext<IdentityUser>
    {
        public SignalRChatContextIdentity(DbContextOptions<SignalRChatContextIdentity> options) : base(options)
        {
            Database.EnsureCreated();
        }

    }
}

using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace P223Chat.Models
{
    public class ChatDbContext:IdentityDbContext
    {
        public ChatDbContext(DbContextOptions<ChatDbContext> options):base(options)
        {

        }

        public DbSet<AppUser> AppUsers { get;set; }
    }
}

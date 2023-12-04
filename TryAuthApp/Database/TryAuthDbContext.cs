using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using TryAuthApp.Models;

namespace TryAuthApp.Database
{
    public class TryAuthDbContext : IdentityDbContext<AppUser>
    {
        public TryAuthDbContext(DbContextOptions options) : base(options)
        {
        }

        public DbSet<Todo> Todos { get; set; }
        public DbSet<TokenInfo> TokenInfo { get; set; }
    }
}

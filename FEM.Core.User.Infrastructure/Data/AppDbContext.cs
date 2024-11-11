using DomainUser = FEM.Core.User.Domain.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace FEM.Core.User.Infrastructure.Data
{
    public class AppDbContext : IdentityDbContext<DomainUser.User>
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.Entity<DomainUser.User>()
            .HasIndex(e => e.Email)
            .IsUnique();
        }
    }
}

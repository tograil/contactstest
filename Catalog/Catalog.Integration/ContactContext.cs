using Catalog.Core.Data;
using Catalog.Core.User;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Catalog.Integration;

public class ContactContext : IdentityDbContext<CatalogUser, IdentityRole<Guid>, Guid>
{
    public ContactContext()
    {
        
    }

    public ContactContext(DbContextOptions<ContactContext> options)
        : base(options)
    {
        
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.Entity<Contact>().HasKey(x => x.Id);
        builder.Entity<Contact>().HasOne(x => x.User)
            .WithMany(x => x.Contacts)
            .HasForeignKey(x => x.UserId);
        builder.Entity<Contact>().Property(x => x.Email).HasMaxLength(255);
        builder.Entity<Contact>().Property(x => x.FirstName).HasMaxLength(255);
        builder.Entity<Contact>().Property(x => x.LastName).HasMaxLength(255);
        builder.Entity<Contact>().Property(x => x.Phone).HasMaxLength(255);
    }
}
using Catalog.Core.Data;
using Microsoft.AspNetCore.Identity;

namespace Catalog.Core.User;

public class CatalogUser : IdentityUser<Guid>
{
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;

    public ICollection<Contact> Contacts { get; set; }
}
using Catalog.Core.Contacts.Dto;
using Catalog.Core.Data;

namespace Catalog.Core.Contacts;

public interface IContactService
{
    Task AddContact(AddContact contactDto);
    Task UpdateContact(UpdateContact contactDto);
    Task DeleteContact(Guid contactId);
    Task<Contact> GetContact(string id);
    Task<ICollection<Contact>> GetContacts(Guid userId);
}
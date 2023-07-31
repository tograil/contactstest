using Catalog.Core.Contacts;
using Catalog.Core.Contacts.Dto;
using Catalog.Core.Data;
using Microsoft.EntityFrameworkCore;

namespace Catalog.Integration.Services;

public class ContactService : IContactService
{
    private readonly ContactContext _context;

    public ContactService(ContactContext context)
    {
        _context = context;
    }

    public async Task AddContact(AddContact contactDto)
    {
        var contact = new Contact
        {
            Email = contactDto.Email,
            UserId = contactDto.UserId,
            FirstName = contactDto.FirstName,
            LastName = contactDto.LastName,
            Phone = contactDto.Phone
        };

        _context.Set<Contact>().Add(contact);

        await _context.SaveChangesAsync();
    }

    public async Task UpdateContact(UpdateContact contactDto)
    {
        var contact = await _context.Set<Contact>().FirstOrDefaultAsync(x => x.Id == contactDto.Id);

        if (contact == null)
        {
            return;
        }

        contact.Email = contactDto.Email;
        contact.FirstName = contactDto.FirstName;
        contact.LastName = contactDto.LastName;
        contact.Phone = contactDto.Phone;

        await _context.SaveChangesAsync();
    }

    public async Task DeleteContact(Guid contactId)
    {
        var contact = await _context.Set<Contact>().FirstOrDefaultAsync(x => x.Id == contactId);

        if (contact == null)
        {
            return;
        }

        _context.Set<Contact>().Remove(contact);

        await _context.SaveChangesAsync();
    }

    public async Task<Contact> GetContact(string id)
    {
        var contact = await _context.Set<Contact>().FirstOrDefaultAsync(x => x.Id == Guid.Parse(id));

        if (contact == null)
        {
            throw new KeyNotFoundException();
        }

        return contact;
    }

    public async Task<ICollection<Contact>> GetContacts(Guid id)
    {
        return await _context.Set<Contact>().Where(x => x.UserId == id).ToListAsync();
    }
}
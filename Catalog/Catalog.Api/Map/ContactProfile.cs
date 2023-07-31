using AutoMapper;
using Catalog.Api.Models;
using Catalog.Core.Data;

namespace Catalog.Api.Map;

public class ContactProfile : Profile
{
    public ContactProfile()
    {
        CreateMap<Contact, ContactModel>();
    }
}
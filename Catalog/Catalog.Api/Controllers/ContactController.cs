using AutoMapper;
using Catalog.Api.Models;
using Catalog.Core.Contacts;
using Catalog.Core.Contacts.Dto;
using Catalog.Core.User;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Catalog.Api.Controllers;

[ApiController]
[Route("/api/[controller]")]
[Authorize(AuthenticationSchemes = "Bearer")]
public class ContactController : Controller
{
    private readonly IContactService _contactService;
    private readonly UserManager<CatalogUser> _userManager;
    private readonly IMapper _mapper;

    public ContactController(IContactService contactService, UserManager<CatalogUser> userManager, IMapper mapper)
    {
        _contactService = contactService;
        _userManager = userManager;
        _mapper = mapper;
    }


    [HttpPost]
    [Route("")]
    public async Task<IActionResult> Add([FromBody] ContactAdd model)
    {
        var name = _userManager.GetUserName(User);

        var user = await _userManager.FindByNameAsync(name);

        if (user == null)
        {
            return NotFound();
        }

        var contact = new AddContact
        {
            Email = model.Email,
            FirstName = model.FirstName,
            LastName = model.LastName,
            Phone = model.Phone,
            UserId = user.Id
        };

        await _contactService.AddContact(contact);

        return Ok();
    }

    [HttpPut]
    [Route("{id:guid}")]
    public async Task<IActionResult> Update(Guid id, [FromBody] ContactUpdate model)
    {
        var name = _userManager.GetUserName(User);

        var user = await _userManager.FindByNameAsync(name);

        if (user == null)
        {
            return NotFound();
        }

        var contact = new UpdateContact
        {
            Email = model.Email,
            FirstName = model.FirstName,
            LastName = model.LastName,
            Phone = model.Phone,
            Id = id
        };

        await _contactService.UpdateContact(contact);

        return Ok();
    }

    [HttpDelete]
    [Route("{id:guid}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        await _contactService.DeleteContact(id);

        return Ok();
    }

    [HttpGet]
    [Route("{id:guid}")]
    public async Task<IActionResult> Get(Guid id)
    {
        try
        {
            var contact = await _contactService.GetContact(id.ToString());

            return Ok(_mapper.Map<ContactModel>(contact));
        }
        catch (Exception)
        {
            return NotFound();
        }
    }

    [HttpGet]
    [Route("")]
    public async Task<IActionResult> Get()
    {
        var name = _userManager.GetUserName(User);

        var user = await _userManager.FindByNameAsync(name);

        if (user == null)
        {
            return NotFound();
        }

        var contacts = await _contactService.GetContacts(user.Id);

        return Ok(_mapper.Map<ICollection<ContactModel>>(contacts));
    }
}
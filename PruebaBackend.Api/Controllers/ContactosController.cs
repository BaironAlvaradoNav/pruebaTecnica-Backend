using Microsoft.AspNetCore.Mvc;
using PruebaBackend.Api.Application.Interfaces;
using PruebaBackend.Api.Contracts;
using PruebaBackend.Api.Domain;

namespace PruebaBackend.Api.Controllers;

[ApiController]
[Route("api/contactos")]
public sealed class ContactosController : ControllerBase
{
    private readonly IContactService _contactService;

    public ContactosController(IContactService contactService)
    {
        _contactService = contactService;
    }

    [HttpGet]
    public ActionResult<IReadOnlyCollection<ContactResponse>> GetAll()
    {
        var contacts = _contactService.GetAll()
            .Select(MapToResponse)
            .ToArray();

        return Ok(contacts);
    }

    [HttpGet("{id:int}")]
    public ActionResult<ContactResponse> GetById(int id)
    {
        var contact = _contactService.GetById(id);

        if (contact is null)
        {
            return NotFound();
        }

        return Ok(MapToResponse(contact));
    }

    [HttpPost]
    public ActionResult<ContactResponse> Create([FromBody] CreateContactRequest request)
    {
        var nombre = request.Nombre?.Trim() ?? string.Empty;
        var telefono = request.Telefono?.Trim() ?? string.Empty;

        if (string.IsNullOrWhiteSpace(nombre) && string.IsNullOrWhiteSpace(telefono))
        {
            return BadRequest(new { error = "Falta digitar nombre y telefono." });
        }

        if (string.IsNullOrWhiteSpace(nombre))
        {
            return BadRequest(new { error = "Falta digitar nombre." });
        }

        if (string.IsNullOrWhiteSpace(telefono))
        {
            return BadRequest(new { error = "Falta digitar telefono." });
        }

        var contact = _contactService.Create(nombre, telefono);

        if (contact is null)
        {
            return Conflict(new { error = "Ya existe un contacto con ese telefono." });
        }

        var response = MapToResponse(contact);
        return CreatedAtAction(nameof(GetById), new { id = response.Id }, response);
    }

    private static ContactResponse MapToResponse(Contact contact)
    {
        return new ContactResponse(contact.Id, contact.Nombre, contact.Telefono);
    }
}

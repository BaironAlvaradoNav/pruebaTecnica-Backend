using PruebaBackend.Api.Application.Interfaces;
using PruebaBackend.Api.Domain;

namespace PruebaBackend.Api.Application.Services;

public sealed class ContactService(IContactRepository repository, ILogger<ContactService> logger) : IContactService
{
    private readonly IContactRepository _repository = repository;
    private readonly ILogger<ContactService> _logger = logger;

    public IReadOnlyCollection<Contact> GetAll() => _repository.GetAll();

    public Contact? GetById(int id) => _repository.GetById(id);

    public Contact? Create(string nombre, string telefono)
    {
        if (!_repository.TryAdd(nombre, telefono, out var contact))
        {
            _logger.LogWarning("No se puede crear contacto. Telefono duplicado: {Telefono}", telefono);
            return null;
        }

        _logger.LogInformation("Contacto creado con Id {Id}", contact!.Id);
        return contact;
    }
}

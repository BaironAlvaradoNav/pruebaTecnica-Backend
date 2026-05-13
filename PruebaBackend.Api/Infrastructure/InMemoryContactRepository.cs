using PruebaBackend.Api.Application.Interfaces;
using PruebaBackend.Api.Domain;

namespace PruebaBackend.Api.Infrastructure;

public sealed class InMemoryContactRepository : IContactRepository
{
    private readonly List<Contact> _contacts = new();
    private readonly object _lock = new();
    private int _nextId = 1;

    public IReadOnlyCollection<Contact> GetAll()
    {
        lock (_lock)
        {
            return _contacts.OrderBy(contact => contact.Id).ToList();
        }
    }

    public Contact? GetById(int id)
    {
        lock (_lock)
        {
            return _contacts.FirstOrDefault(contact => contact.Id == id);
        }
    }

    public bool TryAdd(string nombre, string telefono, out Contact? contact)
    {
        lock (_lock)
        {
            contact = null;
            var normalizedPhone = telefono.Trim();
            var phoneExists = _contacts.Any(existingContact => string.Equals(existingContact.Telefono.Trim(), normalizedPhone, StringComparison.Ordinal));

            if (phoneExists)
            {
                return false;
            }

            var newContact = new Contact(_nextId, nombre, telefono);
            _contacts.Add(newContact);
            _nextId++;

            contact = newContact;
            return true;
        }
    }
}

using PruebaBackend.Api.Domain;

namespace PruebaBackend.Api.Application.Interfaces;

public interface IContactRepository
{
    IReadOnlyCollection<Contact> GetAll();

    Contact? GetById(int id);

    bool TryAdd(string nombre, string telefono, out Contact? contact);
}

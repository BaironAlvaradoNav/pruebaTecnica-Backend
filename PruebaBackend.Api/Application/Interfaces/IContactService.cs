using PruebaBackend.Api.Domain;

namespace PruebaBackend.Api.Application.Interfaces;

public interface IContactService
{
    IReadOnlyCollection<Contact> GetAll();

    Contact? GetById(int id);

    Contact? Create(string nombre, string telefono);
}

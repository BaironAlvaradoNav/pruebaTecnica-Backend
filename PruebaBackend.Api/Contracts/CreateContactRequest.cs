namespace PruebaBackend.Api.Contracts;

public class CreateContactRequest
{
    public string Nombre { get; set; } = string.Empty;

    public string Telefono { get; set; } = string.Empty;
}

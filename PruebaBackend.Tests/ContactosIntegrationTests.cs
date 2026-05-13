using System.Net;
using System.Net.Http.Json;
using Microsoft.AspNetCore.Mvc.Testing;

namespace PruebaBackend.Tests;

public sealed class ContactosIntegrationTests : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly HttpClient _client;

    public ContactosIntegrationTests(WebApplicationFactory<Program> factory)
    {
        _client = factory.CreateClient();
    }

    [Fact]
    public async Task CreateAndGetById_ShouldReturnExpectedResponses()
    {
        var createRequest = new { nombre = "Juan Perez", telefono = "123456789" };
        var createResponse = await _client.PostAsJsonAsync("/api/contactos", createRequest);

        Assert.Equal(HttpStatusCode.Created, createResponse.StatusCode);
        var createdContact = await createResponse.Content.ReadFromJsonAsync<ContactDto>();
        Assert.NotNull(createdContact);

        var getResponse = await _client.GetAsync($"/api/contactos/{createdContact!.Id}");
        Assert.Equal(HttpStatusCode.OK, getResponse.StatusCode);

        var fetchedContact = await getResponse.Content.ReadFromJsonAsync<ContactDto>();
        Assert.NotNull(fetchedContact);
        Assert.Equal(createdContact.Id, fetchedContact!.Id);
        Assert.Equal(createRequest.nombre, fetchedContact.Nombre);
        Assert.Equal(createRequest.telefono, fetchedContact.Telefono);
    }

    [Fact]
    public async Task CreateDuplicatePhone_ShouldReturnConflict()
    {
        var createRequest = new { nombre = "Pedro Soto", telefono = "555111222" };
        await _client.PostAsJsonAsync("/api/contactos", createRequest);
        var duplicateResponse = await _client.PostAsJsonAsync(
            "/api/contactos",
            new { nombre = "Ana Rojas", telefono = "555111222" });

        Assert.Equal(HttpStatusCode.Conflict, duplicateResponse.StatusCode);
    }

    [Fact]
    public async Task CreateWithoutTelefono_ShouldReturnBadRequestWithSpecificMessage()
    {
        var response = await _client.PostAsJsonAsync(
            "/api/contactos",
            new { nombre = "Juan Perez" });

        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        var body = await response.Content.ReadFromJsonAsync<ErrorDto>();
        Assert.NotNull(body);
        Assert.Equal("Falta digitar telefono.", body!.Error);
    }

    [Fact]
    public async Task CreateWithoutNombre_ShouldReturnBadRequestWithSpecificMessage()
    {
        var response = await _client.PostAsJsonAsync(
            "/api/contactos",
            new { telefono = "123456789" });

        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        var body = await response.Content.ReadFromJsonAsync<ErrorDto>();
        Assert.NotNull(body);
        Assert.Equal("Falta digitar nombre.", body!.Error);
    }

    private sealed record ContactDto(int Id, string Nombre, string Telefono);
    private sealed record ErrorDto(string Error);
}

using Microsoft.Extensions.Logging.Abstractions;
using PruebaBackend.Api.Application.Services;
using PruebaBackend.Api.Domain;
using PruebaBackend.Api.Infrastructure;
using System.Collections.Concurrent;

namespace PruebaBackend.Tests;

public sealed class ContactServiceTests
{
    [Fact]
    public void Create_ShouldReturnDuplicatePhone_WhenPhoneAlreadyExists()
    {
        var repository = new InMemoryContactRepository();
        var service = new ContactService(repository, NullLogger<ContactService>.Instance);

        var firstContact = service.Create("Juan Perez", "123456789");
        var duplicateContact = service.Create("Maria Diaz", "123456789");

        Assert.NotNull(firstContact);
        Assert.Null(duplicateContact);
    }

    [Fact]
    public async Task Create_WithParallelDuplicatePhones_ShouldAllowOnlyOneSuccess()
    {
        var repository = new InMemoryContactRepository();
        var service = new ContactService(repository, NullLogger<ContactService>.Instance);
        var results = new ConcurrentBag<Contact?>();

        var tasks = Enumerable.Range(0, 20)
            .Select(index => Task.Run(() =>
                results.Add(service.Create($"Persona {index}", "999888777"))))
            .ToArray();

        await Task.WhenAll(tasks);

        var successCount = results.Count(result => result is not null);
        Assert.Equal(1, successCount);
    }
}

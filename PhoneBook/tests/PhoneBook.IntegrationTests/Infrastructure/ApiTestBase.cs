using Microsoft.AspNetCore.Mvc.Testing;
using System.Net.Http.Json;
using System.Text.Json;

namespace PhoneBook.IntegrationTests.Infrastructure;

public abstract class ApiTestBase : IClassFixture<PhoneBookApiFactory>
{
    protected ApiTestBase(PhoneBookApiFactory factory)
    {
        Factory = factory;

        Client = factory.CreateClient(
            new WebApplicationFactoryClientOptions
            {
                AllowAutoRedirect = false
            });
    }

    protected PhoneBookApiFactory Factory { get; }

    protected HttpClient Client { get; }

    protected static JsonSerializerOptions JsonOptions { get; } =
        new(JsonSerializerDefaults.Web);

    protected static object CreateContactRequest(
        string firstName = "John",
        string lastName = "Doe",
        string phoneNumber = "09121234567",
        string tag = "Work")
    {
        return new
        {
            firstName,
            lastName,
            phoneNumber,
            tag
        };
    }

    protected static object UpdateContactRequest(
        string firstName = "Jane",
        string lastName = "Doe",
        string phoneNumber = "09129876543",
        string tag = "Family")
    {
        return new
        {
            firstName,
            lastName,
            phoneNumber,
            tag
        };
    }

    protected async Task<Guid> CreateContactAndGetIdAsync(
        string firstName = "Integration",
        string lastName = "Test",
        string phoneNumber = "09121111111",
        string tag = "Integration")
    {
        using var response =
            await Client.PostAsJsonAsync(
                "/api/contacts",
                CreateContactRequest(
                    firstName,
                    lastName,
                    phoneNumber,
                    tag));

        response.EnsureSuccessStatusCode();

        var contact = await response.Content.ReadFromJsonAsync<ContactResponse>(JsonOptions);
        return contact!.Id;
    }

    protected sealed record ContactResponse(
        Guid Id,
        string FirstName,
        string LastName,
        string PhoneNumber,
        string Tag);
}

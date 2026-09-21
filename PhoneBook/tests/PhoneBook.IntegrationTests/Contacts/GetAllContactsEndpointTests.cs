using System.Net;
using System.Net.Http.Json;
using PhoneBook.IntegrationTests.Infrastructure;

namespace PhoneBook.IntegrationTests.Contacts;

public sealed class GetAllContactsEndpointTests : ApiTestBase
{
    public GetAllContactsEndpointTests(PhoneBookApiFactory factory) : base(factory)
    {
    }

    [Fact]
    public async Task GetWithoutTag_ShouldReturn200AndAllContacts()
    {
        // Act
        using var response =
            await Client.GetAsync(
                "/api/contacts");

        // Assert
        Assert.Equal(
            HttpStatusCode.OK,
            response.StatusCode);

        var contacts =
            await response.Content.ReadFromJsonAsync<
                List<ContactResponse>>(
                JsonOptions);

        Assert.NotNull(contacts);
        Assert.True(contacts.Count >= 3);

        Assert.Contains(
            contacts,
            contact => contact.FirstName == "Pouya");

        Assert.Contains(
            contacts,
            contact => contact.FirstName == "Ali");

        Assert.Contains(
            contacts,
            contact => contact.FirstName == "Sara");
    }

    [Fact]
    public async Task GetWithoutTag_ShouldReturnJsonContent()
    {
        // Act
        using var response =
            await Client.GetAsync(
                "/api/contacts");

        // Assert
        Assert.Equal(
            "application/json",
            response.Content.Headers.ContentType?.MediaType);
    }
}
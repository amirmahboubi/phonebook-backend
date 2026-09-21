using System.Net;
using System.Net.Http.Json;
using PhoneBook.IntegrationTests.Infrastructure;

namespace PhoneBook.IntegrationTests.Contacts;

public sealed class GetContactByIdEndpointTests : ApiTestBase
{
    public GetContactByIdEndpointTests(PhoneBookApiFactory factory) : base(factory)
    {
    }

    [Fact]
    public async Task GetWithExistingId_ShouldReturn200AndContact()
    {
        // Arrange
        var contactId =
            await CreateContactAndGetIdAsync();

        // Act
        using var response =
            await Client.GetAsync(
                $"/api/contacts/{contactId}");

        // Assert
        Assert.Equal(
            HttpStatusCode.OK,
            response.StatusCode);

        var contact =
            await response.Content.ReadFromJsonAsync<ContactResponse>(
                JsonOptions);

        Assert.NotNull(contact);
        Assert.Equal(
            contactId,
            contact.Id);
    }

    [Fact]
    public async Task GetWithUnknownId_ShouldReturn404()
    {
        // Act
        using var response =
            await Client.GetAsync(
                $"/api/contacts/{Guid.NewGuid()}");

        // Assert
        Assert.Equal(
            HttpStatusCode.NotFound,
            response.StatusCode);

        Assert.Equal(
            "application/problem+json",
            response.Content.Headers.ContentType?.MediaType);
    }

    [Fact]
    public async Task GetWithEmptyGuid_ShouldReturn400()
    {
        // Act
        using var response =
            await Client.GetAsync(
                $"/api/contacts/{Guid.Empty}");

        // Assert
        Assert.Equal(
            HttpStatusCode.BadRequest,
            response.StatusCode);
    }
}

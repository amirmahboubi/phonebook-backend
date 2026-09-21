using PhoneBook.IntegrationTests.Infrastructure;
using System.Net;

namespace PhoneBook.IntegrationTests.Contacts;

public sealed class DeleteContactEndpointTests : ApiTestBase
{
    public DeleteContactEndpointTests(PhoneBookApiFactory factory) : base(factory)
    {
    }

    [Fact]
    public async Task Delete_WithExistingId_ShouldReturn204()
    {
        // Arrange
        var contactId =
            await CreateContactAndGetIdAsync();

        // Act
        using var response =
            await Client.DeleteAsync(
                $"/api/contacts/{contactId}");

        // Assert
        Assert.Equal(
            HttpStatusCode.NoContent,
            response.StatusCode);
    }

    [Fact]
    public async Task Delete_ShouldRemoveContact()
    {
        // Arrange
        var contactId =
            await CreateContactAndGetIdAsync();

        // Act
        using var deleteResponse =
            await Client.DeleteAsync(
                $"/api/contacts/{contactId}");

        // Assert delete
        Assert.Equal(
            HttpStatusCode.NoContent,
            deleteResponse.StatusCode);

        // Verify resource no longer exists
        using var getResponse =
            await Client.GetAsync(
                $"/api/contacts/{contactId}");

        Assert.Equal(
            HttpStatusCode.NotFound,
            getResponse.StatusCode);
    }

    [Fact]
    public async Task Delete_WithUnknownId_ShouldReturn404()
    {
        // Act
        using var response =
            await Client.DeleteAsync(
                $"/api/contacts/{Guid.NewGuid()}");

        // Assert
        Assert.Equal(
            HttpStatusCode.NotFound,
            response.StatusCode);
    }

    [Fact]
    public async Task Delete_WithEmptyGuid_ShouldReturn400()
    {
        // Act
        using var response =
            await Client.DeleteAsync(
                $"/api/contacts/{Guid.Empty}");

        // Assert
        Assert.Equal(
            HttpStatusCode.BadRequest,
            response.StatusCode);
    }
}

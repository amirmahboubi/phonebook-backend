using System.Net;
using System.Net.Http.Json;
using PhoneBook.IntegrationTests.Infrastructure;

namespace PhoneBook.IntegrationTests.Contacts;

public sealed class UpdateContactEndpointTests : ApiTestBase
{
    public UpdateContactEndpointTests(PhoneBookApiFactory factory) : base(factory)
    {
    }

    [Fact]
    public async Task Put_WithExistingId_ShouldReturn204()
    {
        // Arrange
        var contactId =
            await CreateContactAndGetIdAsync();

        var request =
            UpdateContactRequest(
                "Ali",
                "Ahmadi",
                "09129876543",
                "Family");

        // Act
        using var response =
            await Client.PutAsJsonAsync(
                $"/api/contacts/{contactId}",
                request);

        // Assert
        Assert.Equal(
            HttpStatusCode.NoContent,
            response.StatusCode);
    }

    [Fact]
    public async Task Put_ShouldUpdateContact()
    {
        // Arrange
        var contactId =
            await CreateContactAndGetIdAsync();

        var request =
            UpdateContactRequest(
                "Ali",
                "Ahmadi",
                "09129876543",
                "Family");

        // Act
        using var updateResponse =
            await Client.PutAsJsonAsync(
                $"/api/contacts/{contactId}",
                request);

        // Assert update
        Assert.Equal(
            HttpStatusCode.NoContent,
            updateResponse.StatusCode);

        // Verify persisted state
        using var getResponse =
            await Client.GetAsync(
                $"/api/contacts/{contactId}");

        var contact =
            await getResponse.Content.ReadFromJsonAsync<ContactResponse>(
                JsonOptions);

        Assert.NotNull(contact);
        Assert.Equal("Ali", contact.FirstName);
        Assert.Equal("Ahmadi", contact.LastName);
        Assert.Equal("09129876543", contact.PhoneNumber);
        Assert.Equal("Family", contact.Tag);
        Assert.Equal(contactId, contact.Id);
    }

    [Fact]
    public async Task Put_WithUnknownId_ShouldReturn404()
    {
        // Arrange
        var request =
            UpdateContactRequest();

        // Act
        using var response =
            await Client.PutAsJsonAsync(
                $"/api/contacts/{Guid.NewGuid()}",
                request);

        // Assert
        Assert.Equal(
            HttpStatusCode.NotFound,
            response.StatusCode);
    }

    [Fact]
    public async Task Put_WithEmptyGuid_ShouldReturn400()
    {
        // Arrange
        var request =
            UpdateContactRequest();

        // Act
        using var response =
            await Client.PutAsJsonAsync(
                $"/api/contacts/{Guid.Empty}",
                request);

        // Assert
        Assert.Equal(
            HttpStatusCode.BadRequest,
            response.StatusCode);
    }

    [Fact]
    public async Task Put_WithInvalidRequest_ShouldReturn400()
    {
        // Arrange
        var contactId =
            await CreateContactAndGetIdAsync();

        var request = new
        {
            firstName = "",
            lastName = "",
            phoneNumber = " ",
            tag = ""
        };

        // Act
        using var response =
            await Client.PutAsJsonAsync(
                $"/api/contacts/{contactId}",
                request);

        // Assert
        Assert.Equal(
            HttpStatusCode.BadRequest,
            response.StatusCode);

        Assert.Equal(
            "application/problem+json",
            response.Content.Headers.ContentType?.MediaType);
    }

    [Fact]
    public async Task Put_ShouldNotChangeContactWhenRequestIsInvalid()
    {
        // Arrange
        var contactId =
            await CreateContactAndGetIdAsync(
                "Original",
                "Contact",
                "09121234567",
                "Work");

        var invalidRequest = new
        {
            firstName = "Changed",
            lastName = "Changed",
            phoneNumber = "",
            tag = "Friend"
        };

        // Act
        using var response =
            await Client.PutAsJsonAsync(
                $"/api/contacts/{contactId}",
                invalidRequest);

        // Assert
        Assert.Equal(
            HttpStatusCode.BadRequest,
            response.StatusCode);

        using var getResponse =
            await Client.GetAsync(
                $"/api/contacts/{contactId}");

        var contact =
            await getResponse.Content.ReadFromJsonAsync<ContactResponse>(
                JsonOptions);

        Assert.NotNull(contact);
        Assert.Equal("Original", contact.FirstName);
        Assert.Equal("Contact", contact.LastName);
        Assert.Equal("09121234567", contact.PhoneNumber);
        Assert.Equal("Work", contact.Tag);
    }
}
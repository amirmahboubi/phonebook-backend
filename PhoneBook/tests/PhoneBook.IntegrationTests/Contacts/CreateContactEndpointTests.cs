using PhoneBook.IntegrationTests.Infrastructure;
using System.Net;
using System.Net.Http.Json;

namespace PhoneBook.IntegrationTests.Contacts;

public sealed class CreateContactEndpointTests : ApiTestBase
{
    public CreateContactEndpointTests(PhoneBookApiFactory factory) : base(factory)
    {
    }

    [Fact]
    public async Task Post_WithValidRequest_ShouldReturn201Created()
    {
        // Arrange
        var request = CreateContactRequest(
            "Pouya",
            "Mahboubi",
            "09121234567",
            "Work");

        // Act
        using var response =
            await Client.PostAsJsonAsync(
                "/api/contacts",
                request);

        // Assert
        Assert.Equal(
            HttpStatusCode.Created,
            response.StatusCode);

        var contact =
            await response.Content.ReadFromJsonAsync<ContactResponse>(
                JsonOptions);

        Assert.NotNull(contact);
        Assert.NotEqual(Guid.Empty, contact.Id);
        Assert.Equal("Pouya", contact.FirstName);
        Assert.Equal("Mahboubi", contact.LastName);
        Assert.Equal("09121234567", contact.PhoneNumber);
        Assert.Equal("Work", contact.Tag);

        Assert.NotNull(response.Headers.Location);

        var expectedLocation = new Uri(
            Client.BaseAddress!,
            $"/api/contacts/{contact.Id}");

        Assert.Equal(
            expectedLocation,
            response.Headers.Location);
    }

    [Fact]
    public async Task Post_WithInvalidRequest_ShouldReturn400ProblemDetails()
    {
        // Arrange
        var request = new
        {
            firstName = "",
            lastName = " ",
            phoneNumber = "",
            tag = ""
        };

        // Act
        using var response =
            await Client.PostAsJsonAsync(
                "/api/contacts",
                request);

        // Assert
        Assert.Equal(
            HttpStatusCode.BadRequest,
            response.StatusCode);

        Assert.Equal(
            "application/problem+json",
            response.Content.Headers.ContentType?.MediaType);

        var problem =
            await response.Content.ReadFromJsonAsync<ProblemDetailsResponse>(
                JsonOptions);

        Assert.NotNull(problem);
        Assert.Equal(
            400,
            problem.Status);

        Assert.Equal(
            "Validation failed.",
            problem.Title);
    }

    private sealed record ProblemDetailsResponse(
        int? Status,
        string? Title,
        string? Detail);
}

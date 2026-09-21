using System.Net;
using System.Net.Http.Json;

namespace PhoneBook.IntegrationTests.Infrastructure;

public sealed class GlobalErrorHandlingTests : ApiTestBase
{
    public GlobalErrorHandlingTests(PhoneBookApiFactory factory) : base(factory)
    {
    }

    [Fact]
    public async Task ValidationFailure_ShouldReturnProblemDetails()
    {
        // Arrange
        var request = new
        {
            firstName = "",
            lastName = "",
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

        var body =
            await response.Content.ReadAsStringAsync();

        Assert.Contains(
            "Validation failed.",
            body);
    }

    [Fact]
    public async Task NotFoundFailure_ShouldReturnProblemDetails()
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

        var body =
            await response.Content.ReadAsStringAsync();

        Assert.Contains(
            "not found",
            body,
            StringComparison.OrdinalIgnoreCase);
    }
}
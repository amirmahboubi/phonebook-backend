using System.Net;

namespace PhoneBook.IntegrationTests.Infrastructure;

public sealed class OpenApiTests : ApiTestBase
{
    public OpenApiTests(PhoneBookApiFactory factory) : base(factory)
    {
    }

    [Fact]
    public async Task OpenApiDocument_ShouldBeAvailable()
    {
        // Act
        using var response =
            await Client.GetAsync(
                "/openapi/v1.json");

        // Assert
        Assert.Equal(
            HttpStatusCode.OK,
            response.StatusCode);

        Assert.Equal(
            "application/json",
            response.Content.Headers.ContentType?.MediaType);

        var document =
            await response.Content.ReadAsStringAsync();

        Assert.Contains(
            "/api/contacts",
            document);

        Assert.Contains(
            "post",
            document,
            StringComparison.OrdinalIgnoreCase);

        Assert.Contains(
            "get",
            document,
            StringComparison.OrdinalIgnoreCase);

        Assert.Contains(
            "put",
            document,
            StringComparison.OrdinalIgnoreCase);

        Assert.Contains(
            "delete",
            document,
            StringComparison.OrdinalIgnoreCase);
    }
}

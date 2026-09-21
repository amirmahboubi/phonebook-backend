using System.Net;
using System.Text.Json;

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

        await using var stream =
            await response.Content.ReadAsStreamAsync();

        using var document =
            await JsonDocument.ParseAsync(stream);

        var root = document.RootElement;

        Assert.True(
            root.TryGetProperty(
                "openapi",
                out var openApiVersion));

        Assert.False(
            string.IsNullOrWhiteSpace(
                openApiVersion.GetString()));

        Assert.True(
            root.TryGetProperty(
                "paths",
                out var paths));

        Assert.True(
            paths.TryGetProperty(
                "/api/contacts/",
                out _)
            || paths.TryGetProperty(
                "/api/contacts",
                out _));

        Assert.True(
            paths.TryGetProperty(
                "/api/contacts/{id}",
                out _));
    }
}
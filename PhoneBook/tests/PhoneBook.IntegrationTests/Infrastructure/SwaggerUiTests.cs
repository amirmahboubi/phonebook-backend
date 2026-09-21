using System.Net;

namespace PhoneBook.IntegrationTests.Infrastructure;

public sealed class SwaggerUiTests : ApiTestBase
{
    public SwaggerUiTests(PhoneBookApiFactory factory) : base(factory)
    {
    }

    [Fact]
    public async Task SwaggerUi_ShouldBeAvailable()
    {
        // Act
        using var response =
            await Client.GetAsync(
                "/swagger/index.html");

        // Assert
        Assert.Equal(
            HttpStatusCode.OK,
            response.StatusCode);

        Assert.Equal(
            "text/html",
            response.Content.Headers.ContentType?.MediaType);
    }
}

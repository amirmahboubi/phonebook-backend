using System.Net;
using System.Net.Http.Json;
using PhoneBook.IntegrationTests.Infrastructure;

namespace PhoneBook.IntegrationTests.Contacts;

public sealed class GetContactsByTagEndpointTests : ApiTestBase
{
    public GetContactsByTagEndpointTests(PhoneBookApiFactory factory) : base(factory)
    {
    }

    [Fact]
    public async Task GetWithTag_ShouldReturnOnlyMatchingContacts()
    {
        // Act
        using var response =
            await Client.GetAsync(
                "/api/contacts?tag=Work");

        // Assert
        Assert.Equal(
            HttpStatusCode.OK,
            response.StatusCode);

        var contacts =
            await response.Content.ReadFromJsonAsync<
                List<ContactResponse>>(
                JsonOptions);

        Assert.NotNull(contacts);
        Assert.NotEmpty(contacts);

        Assert.All(
            contacts,
            contact => Assert.Equal(
                "Work",
                contact.Tag));
    }

    [Fact]
    public async Task GetWithTagWithDifferentCase_ShouldRespectCaseSensitivity()
    {
        // Act
        using var response =
            await Client.GetAsync(
                "/api/contacts?tag=work");

        // Assert
        Assert.Equal(
            HttpStatusCode.OK,
            response.StatusCode);

        var contacts =
            await response.Content.ReadFromJsonAsync<
                List<ContactResponse>>(
                JsonOptions);

        Assert.Empty(contacts!);
    }

    [Fact]
    public async Task GetWithUnknownTag_ShouldReturn200WithEmptyCollection()
    {
        // Act
        using var response =
            await Client.GetAsync(
                "/api/contacts?tag=DoesNotExist");

        // Assert
        Assert.Equal(
            HttpStatusCode.OK,
            response.StatusCode);

        var contacts =
            await response.Content.ReadFromJsonAsync<
                List<ContactResponse>>(
                JsonOptions);

        Assert.NotNull(contacts);
        Assert.Empty(contacts);
    }

    [Fact]
    public async Task GetWithEmptyTag_ShouldReturn400()
    {
        // Act
        using var response =
            await Client.GetAsync(
                "/api/contacts?tag=");

        // Assert
        Assert.Equal(
            HttpStatusCode.BadRequest,
            response.StatusCode);

        Assert.Equal(
            "application/problem+json",
            response.Content.Headers.ContentType?.MediaType);
    }

    [Fact]
    public async Task GetWithTrimmedTag_ShouldReturnMatchingContacts()
    {
        // Act
        using var response =
            await Client.GetAsync(
                "/api/contacts?tag=%20Work%20");

        // Assert
        Assert.Equal(
            HttpStatusCode.OK,
            response.StatusCode);

        var contacts =
            await response.Content.ReadFromJsonAsync<
                List<ContactResponse>>(
                JsonOptions);

        Assert.NotEmpty(contacts!);

        Assert.All(
            contacts!,
            contact => Assert.Equal(
                "Work",
                contact.Tag));
    }
}

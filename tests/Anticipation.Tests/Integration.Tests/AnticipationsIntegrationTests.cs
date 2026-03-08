using System.Net;
using System.Net.Http.Json;
using System.Text.Json;
using Microsoft.AspNetCore.Mvc.Testing;

namespace Anticipation.Tests.Integration.Tests;

public sealed class AnticipationsIntegrationTests : IClassFixture<CustomWebApplicationFactory>
{
    private readonly HttpClient _client;

    public AnticipationsIntegrationTests(CustomWebApplicationFactory factory)
    {
        _client = factory.CreateClient(new WebApplicationFactoryClientOptions
        {
            BaseAddress = new Uri("https://localhost")
        });
    }

    [Fact]
    public async Task Create_Should_Return_Created_With_NetAmount_And_DefaultStatus()
    {
        var payload = new
        {
            creator_id = $"creator-{Guid.NewGuid():N}",
            valor_solicitado = 1000.00m,
            data_solicitacao = DateTime.UtcNow
        };

        var response = await _client.PostAsJsonAsync("/api/v1/anticipations", payload);

        Assert.Equal(HttpStatusCode.Created, response.StatusCode);

        using var document = JsonDocument.Parse(await response.Content.ReadAsStringAsync());
        var root = document.RootElement;

        Assert.Equal(1000.00m, root.GetProperty("valor_solicitado").GetDecimal());
        Assert.Equal(950.00m, root.GetProperty("valor_liquido").GetDecimal());
        Assert.Equal("pendentef", root.GetProperty("status").GetString());
    }

    [Fact]
    public async Task Create_Should_Return_BadRequest_When_Creator_Already_Has_Pending_Request()
    {
        var creatorId = $"creator-{Guid.NewGuid():N}";

        var firstPayload = new
        {
            creator_id = creatorId,
            valor_solicitado = 1200.00m,
            data_solicitacao = DateTime.UtcNow
        };

        var secondPayload = new
        {
            creator_id = creatorId,
            valor_solicitado = 1300.00m,
            data_solicitacao = DateTime.UtcNow.AddMinutes(1)
        };

        var firstResponse = await _client.PostAsJsonAsync("/api/v1/anticipations", firstPayload);
        var secondResponse = await _client.PostAsJsonAsync("/api/v1/anticipations", secondPayload);

        Assert.Equal(HttpStatusCode.Created, firstResponse.StatusCode);
        Assert.Equal(HttpStatusCode.BadRequest, secondResponse.StatusCode);
    }

    [Fact]
    public async Task GetAll_Should_Return_Paged_Metadata_And_Respect_PageSize()
    {
        var testRunId = Guid.NewGuid().ToString("N");

        for (var i = 0; i < 3; i++)
        {
            var payload = new
            {
                creator_id = $"creator-{testRunId}-{i}",
                valor_solicitado = 1000.00m + i,
                data_solicitacao = DateTime.UtcNow.AddMinutes(i)
            };

            var createResponse = await _client.PostAsJsonAsync("/api/v1/anticipations", payload);
            Assert.Equal(HttpStatusCode.Created, createResponse.StatusCode);
        }

        var response = await _client.GetAsync("/api/v1/anticipations?page=1&pageSize=2");
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        using var document = JsonDocument.Parse(await response.Content.ReadAsStringAsync());
        var root = document.RootElement;
        var items = root.GetProperty("items");

        Assert.Equal(1, root.GetProperty("page").GetInt32());
        Assert.Equal(2, root.GetProperty("pageSize").GetInt32());
        Assert.True(root.GetProperty("totalItems").GetInt32() >= 3);
        Assert.True(root.GetProperty("totalPages").GetInt32() >= 2);
        Assert.Equal(2, items.GetArrayLength());
    }
}
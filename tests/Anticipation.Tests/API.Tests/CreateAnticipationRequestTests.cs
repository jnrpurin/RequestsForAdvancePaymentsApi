using Anticipation.API.Contracts;

namespace Anticipation.Tests.API.Tests;

public class CreateAnticipationRequestTests
{
    [Fact]
    public void Should_Create_Request_Contract()
    {
        var request = new CreateAnticipationRequest("creator-1", 300m, "BRL");

        Assert.Equal("creator-1", request.CreatorId);
        Assert.Equal(300m, request.Amount);
        Assert.Equal("BRL", request.Currency);
    }
}
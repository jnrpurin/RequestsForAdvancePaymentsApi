using Anticipation.Application.Commands;

namespace Anticipation.Tests.Application.Tests;

public class CreateAnticipationCommandTests
{
    [Fact]
    public void Should_Create_Command_With_Expected_Data()
    {
        var command = new CreateAnticipationCommand("creator-1", 1500m, "BRL");

        Assert.Equal("creator-1", command.CreatorId);
        Assert.Equal(1500m, command.Amount);
        Assert.Equal("BRL", command.Currency);
    }
}
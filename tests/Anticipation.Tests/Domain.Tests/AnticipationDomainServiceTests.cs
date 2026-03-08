using Anticipation.Domain.Enums;
using Anticipation.Domain.Services;

namespace Anticipation.Tests.Domain.Tests;

public class AnticipationDomainServiceTests
{
    private readonly AnticipationDomainService _service = new();

    [Fact]
    public void Create_Should_Throw_When_RequestedAmount_Is_Less_Than_Or_Equal_100()
    {
        var requestDate = new DateTime(2026, 3, 8, 10, 0, 0, DateTimeKind.Utc);

        var act = () => _service.Create("creator-1", 100.00m, requestDate);

        Assert.Throws<ArgumentException>(act);
    }

    [Fact]
    public void Create_Should_Apply_FivePercent_Fee_And_Start_As_Pending()
    {
        var requestDate = new DateTime(2026, 3, 8, 10, 0, 0, DateTimeKind.Utc);

        var request = _service.Create("creator-1", 1000.00m, requestDate);

        Assert.Equal(1000.00m, request.RequestedAmount.Amount);
        Assert.Equal(950.00m, request.NetAmount);
        Assert.Equal(RequestStatus.Pending, request.Status);
    }
}
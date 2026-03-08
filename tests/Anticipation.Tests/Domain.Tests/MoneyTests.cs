using Anticipation.Domain.ValueObjects;

namespace Anticipation.Tests.Domain.Tests;

public class MoneyTests
{
    [Fact]
    public void Should_Throw_When_Amount_Is_Invalid()
    {
        Assert.Throws<ArgumentException>(() => new Money(0, "BRL"));
    }
}
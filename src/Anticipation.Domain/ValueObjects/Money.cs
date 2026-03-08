namespace Anticipation.Domain.ValueObjects;

public sealed class Money
{
    public decimal Amount { get; private set; }
    public string Currency { get; private set; } = string.Empty;

    private Money()
    {
    }

    public Money(decimal amount, string currency)
    {
        if (amount <= 0)
        {
            throw new ArgumentException("Amount must be greater than zero.", nameof(amount));
        }

        if (string.IsNullOrWhiteSpace(currency) || currency.Trim().Length != 3)
        {
            throw new ArgumentException("Currency must be a 3-letter ISO code.", nameof(currency));
        }

        Amount = decimal.Round(amount, 2, MidpointRounding.AwayFromZero);
        Currency = currency.Trim().ToUpperInvariant();
    }
}
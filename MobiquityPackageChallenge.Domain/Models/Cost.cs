namespace MobiquityPackageChallenge.Domain.Models;

public class Cost
{
    public char CurrencySymbol { get; private set; }
    public double Amount { get; private set; }

    public Cost(char currencySymbol, double amount)
    {
        CurrencySymbol = currencySymbol;
        Amount = amount;
    }
}

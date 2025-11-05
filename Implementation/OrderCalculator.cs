using System;
using System.Linq;

public class OrderCalculator
{
    private readonly decimal _taxRate;
    private readonly decimal _onePlusTaxRate;

    public OrderCalculator(decimal taxRate)
    {
        if (taxRate < 0)
        {
            throw new ArgumentException("Tax rate cannot be negative.");
        }
        _taxRate = taxRate;
        _onePlusTaxRate = 1 + _taxRate;
    }

    public void Calculate(Order order)
    {
        foreach (var item in order.Items)
        {
            item.NetTotal = item.NetPrice * item.Quantity;
            item.Total = Math.Round(item.NetTotal * _onePlusTaxRate, 2, MidpointRounding.AwayFromZero);
        }

        order.NetTotal = order.Items.Sum(item => item.NetTotal);
        order.Total = order.Items.Sum(item => item.Total);
        order.Tax = order.Total - order.NetTotal;
    }
}

using System.Collections.Generic;
using System.Globalization;
using System.Text;

public class Order
{
    public decimal NetTotal { get; set; }
    public decimal Tax { get; set; }
    public decimal Total { get; set; }

    public List<OrderItem> Items { get; }

    public Order()
    {
        Items = new List<OrderItem>();
    }

    public override string ToString()
    {
        var culture = new CultureInfo("pl-PL");
        var sb = new StringBuilder();
        sb.AppendLine($"Order (Net: {NetTotal.ToString("C", culture)}, Tax: {Tax.ToString("C", culture)}, Total: {Total.ToString("C", culture)})");
        foreach (var item in Items)
        {
            sb.AppendLine($"  -> {item}");
        }
        return sb.ToString();
    }
}

using System.Globalization;

public class OrderItem
{
    public decimal NetPrice { get; }
    public int Quantity { get; }

    public decimal NetTotal { get; set; }
    public decimal Total { get; set; }

    public OrderItem(decimal netPrice, int quantity)
    {
        NetPrice = netPrice;
        Quantity = quantity;
    }

    public override string ToString()
    {
        var culture = new CultureInfo("pl-PL");
        return $"Item (Price: {NetPrice.ToString("C", culture)}, Qty: {Quantity}, NetTotal: {NetTotal.ToString("C", culture)}, Total: {Total.ToString("C", culture)})";
    }
}

using System;

class Program
{
    static void Main(string[] args)
    {
        var calculator = new OrderCalculator(0.23m);
        var order = new Order();
        
        order.Items.Add(new OrderItem(10.00m, 2));
        order.Items.Add(new OrderItem(5.50m, 1));
        
        calculator.Calculate(order);
        
        Console.WriteLine("Order Calculation Results:");
        Console.WriteLine(order.ToString());
    }
}

using System;
using Xunit;

namespace OrderCalculatorTests
{
    public class OrderCalculatorTests
    {
        [Fact]
        public void Calculate_SingleItem_CalculatesCorrectly()
        {
            // Arrange
            var calculator = new OrderCalculator(0.23m);
            var order = new Order();
            order.Items.Add(new OrderItem(10.00m, 2));

            // Act
            calculator.Calculate(order);

            // Assert
            Assert.Equal(20.00m, order.Items[0].NetTotal);
            Assert.Equal(24.60m, order.Items[0].Total);
            Assert.Equal(20.00m, order.NetTotal);
            Assert.Equal(24.60m, order.Total);
            Assert.Equal(4.60m, order.Tax);
        }

        [Fact]
        public void Calculate_MultipleItems_CalculatesCorrectly()
        {
            // Arrange
            var calculator = new OrderCalculator(0.23m);
            var order = new Order();
            order.Items.Add(new OrderItem(10.00m, 2)); // Net: 20.00, Total: 24.60
            order.Items.Add(new OrderItem(5.50m, 1));  // Net: 5.50, Total: 6.77

            // Act
            calculator.Calculate(order);

            // Assert
            // Item 1
            Assert.Equal(20.00m, order.Items[0].NetTotal);
            Assert.Equal(24.60m, order.Items[0].Total);
            
            // Item 2
            Assert.Equal(5.50m, order.Items[1].NetTotal);
            Assert.Equal(6.77m, order.Items[1].Total);
            
            // Order totals
            Assert.Equal(25.50m, order.NetTotal);
            Assert.Equal(31.37m, order.Total);
            Assert.Equal(5.87m, order.Tax);
        }

        [Fact]
        public void Calculate_EmptyOrder_ReturnsZeroValues()
        {
            // Arrange
            var calculator = new OrderCalculator(0.23m);
            var order = new Order();

            // Act
            calculator.Calculate(order);

            // Assert
            Assert.Equal(0m, order.NetTotal);
            Assert.Equal(0m, order.Total);
            Assert.Equal(0m, order.Tax);
        }

        [Fact]
        public void Calculate_ZeroTaxRate_CalculatesCorrectly()
        {
            // Arrange
            var calculator = new OrderCalculator(0m);
            var order = new Order();
            order.Items.Add(new OrderItem(100.00m, 1));

            // Act
            calculator.Calculate(order);

            // Assert
            Assert.Equal(100.00m, order.Items[0].NetTotal);
            Assert.Equal(100.00m, order.Items[0].Total);
            Assert.Equal(100.00m, order.NetTotal);
            Assert.Equal(100.00m, order.Total);
            Assert.Equal(0m, order.Tax);
        }

        [Fact]
        public void Calculate_HighTaxRate_CalculatesCorrectly()
        {
            // Arrange - 50% tax rate
            var calculator = new OrderCalculator(0.50m);
            var order = new Order();
            order.Items.Add(new OrderItem(100.00m, 1));

            // Act
            calculator.Calculate(order);

            // Assert
            Assert.Equal(100.00m, order.Items[0].NetTotal);
            Assert.Equal(150.00m, order.Items[0].Total);
            Assert.Equal(100.00m, order.NetTotal);
            Assert.Equal(150.00m, order.Total);
            Assert.Equal(50.00m, order.Tax);
        }

        [Fact]
        public void Constructor_NegativeTaxRate_ThrowsArgumentException()
        {
            // Act & Assert
            Assert.Throws<ArgumentException>(() => new OrderCalculator(-0.23m));
        }

        [Fact]
        public void Calculate_ItemWithZeroQuantity_CalculatesCorrectly()
        {
            // Arrange
            var calculator = new OrderCalculator(0.23m);
            var order = new Order();
            order.Items.Add(new OrderItem(10.00m, 0));

            // Act
            calculator.Calculate(order);

            // Assert
            Assert.Equal(0m, order.Items[0].NetTotal);
            Assert.Equal(0m, order.Items[0].Total);
            Assert.Equal(0m, order.NetTotal);
            Assert.Equal(0m, order.Total);
            Assert.Equal(0m, order.Tax);
        }

        [Fact]
        public void Calculate_ItemWithZeroPrice_CalculatesCorrectly()
        {
            // Arrange
            var calculator = new OrderCalculator(0.23m);
            var order = new Order();
            order.Items.Add(new OrderItem(0m, 5));

            // Act
            calculator.Calculate(order);

            // Assert
            Assert.Equal(0m, order.Items[0].NetTotal);
            Assert.Equal(0m, order.Items[0].Total);
            Assert.Equal(0m, order.NetTotal);
            Assert.Equal(0m, order.Total);
            Assert.Equal(0m, order.Tax);
        }

        [Fact]
        public void Calculate_RoundingScenario_RoundsCorrectly()
        {
            // Arrange - Testing rounding with 23% tax on 10.01
            var calculator = new OrderCalculator(0.23m);
            var order = new Order();
            order.Items.Add(new OrderItem(10.01m, 1)); // 10.01 * 1.23 = 12.3123

            // Act
            calculator.Calculate(order);

            // Assert
            Assert.Equal(10.01m, order.Items[0].NetTotal);
            Assert.Equal(12.31m, order.Items[0].Total); // Should round to 12.31
            Assert.Equal(10.01m, order.NetTotal);
            Assert.Equal(12.31m, order.Total);
            Assert.Equal(2.30m, order.Tax);
        }

        [Fact]
        public void Calculate_LargeQuantity_CalculatesCorrectly()
        {
            // Arrange
            var calculator = new OrderCalculator(0.23m);
            var order = new Order();
            order.Items.Add(new OrderItem(1.99m, 100));

            // Act
            calculator.Calculate(order);

            // Assert
            Assert.Equal(199.00m, order.Items[0].NetTotal);
            Assert.Equal(244.77m, order.Items[0].Total);
            Assert.Equal(199.00m, order.NetTotal);
            Assert.Equal(244.77m, order.Total);
            Assert.Equal(45.77m, order.Tax);
        }

        [Fact]
        public void Calculate_DecimalPrices_CalculatesCorrectly()
        {
            // Arrange
            var calculator = new OrderCalculator(0.23m);
            var order = new Order();
            order.Items.Add(new OrderItem(3.33m, 3)); // 9.99 net

            // Act
            calculator.Calculate(order);

            // Assert
            Assert.Equal(9.99m, order.Items[0].NetTotal);
            Assert.Equal(12.29m, order.Items[0].Total); // 9.99 * 1.23 = 12.2877 -> 12.29
            Assert.Equal(9.99m, order.NetTotal);
            Assert.Equal(12.29m, order.Total);
            Assert.Equal(2.30m, order.Tax);
        }

        [Fact]
        public void Calculate_PolishVATRate_CalculatesCorrectly()
        {
            // Arrange - Standard Polish VAT rate
            var calculator = new OrderCalculator(0.23m);
            var order = new Order();
            order.Items.Add(new OrderItem(100.00m, 1));
            order.Items.Add(new OrderItem(50.00m, 2));
            order.Items.Add(new OrderItem(25.00m, 4));

            // Act
            calculator.Calculate(order);

            // Assert
            // Item 1: 100 * 1 = 100 net, 123 gross
            Assert.Equal(100.00m, order.Items[0].NetTotal);
            Assert.Equal(123.00m, order.Items[0].Total);
            
            // Item 2: 50 * 2 = 100 net, 123 gross
            Assert.Equal(100.00m, order.Items[1].NetTotal);
            Assert.Equal(123.00m, order.Items[1].Total);
            
            // Item 3: 25 * 4 = 100 net, 123 gross
            Assert.Equal(100.00m, order.Items[2].NetTotal);
            Assert.Equal(123.00m, order.Items[2].Total);
            
            // Order: 300 net, 369 gross, 69 tax
            Assert.Equal(300.00m, order.NetTotal);
            Assert.Equal(369.00m, order.Total);
            Assert.Equal(69.00m, order.Tax);
        }

        [Fact]
        public void Calculate_MixedPricesAndQuantities_CalculatesCorrectly()
        {
            // Arrange
            var calculator = new OrderCalculator(0.08m); // 8% tax (reduced rate)
            var order = new Order();
            order.Items.Add(new OrderItem(15.99m, 3));
            order.Items.Add(new OrderItem(7.50m, 5));
            order.Items.Add(new OrderItem(99.99m, 1));

            // Act
            calculator.Calculate(order);

            // Assert
            // Item 1: 15.99 * 3 = 47.97 net
            Assert.Equal(47.97m, order.Items[0].NetTotal);
            Assert.Equal(51.81m, order.Items[0].Total); // 47.97 * 1.08 = 51.8076 -> 51.81
            
            // Item 2: 7.50 * 5 = 37.50 net
            Assert.Equal(37.50m, order.Items[1].NetTotal);
            Assert.Equal(40.50m, order.Items[1].Total); // 37.50 * 1.08 = 40.50
            
            // Item 3: 99.99 * 1 = 99.99 net
            Assert.Equal(99.99m, order.Items[2].NetTotal);
            Assert.Equal(107.99m, order.Items[2].Total); // 99.99 * 1.08 = 107.9892 -> 107.99
            
            // Order totals
            Assert.Equal(185.46m, order.NetTotal);
            Assert.Equal(200.30m, order.Total);
            Assert.Equal(14.84m, order.Tax);
        }
    }
}


namespace TemplatingEngines.Common;

public class Order {
	private const decimal TAX_RATE = 0.2m;
	public string OrderId { get; set; } = String.Empty;
	public string CustomerName { get; set; } = String.Empty;
	public string CustomerEmail { get; set; } = String.Empty;
	public decimal ShippingCost { get; set; }
	public Address ShippingAddress { get; set; } = new();
	public List<OrderItem> Items { get; set; } = [];
	public decimal Subtotal => Items.Sum(item => item.Total);
	public decimal TaxAmount => Subtotal * TAX_RATE;
	public decimal TotalCost => Subtotal + TaxAmount + ShippingCost;
}

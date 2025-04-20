namespace TemplatingEngines.Common;

public class OrderItem() {
	public OrderItem(string name, int quantity, decimal unitPrice) : this() {
		Name = name;
		Quantity = quantity;
		UnitPrice = unitPrice;
	}

	public int Quantity { get; set; }
	public string Name { get; set; } = String.Empty;
	public decimal UnitPrice { get; set; }
	public decimal Total => UnitPrice * Quantity;
}

using System.Text;
using TemplatingEngines.Common;

namespace TemplatingEngines.Services
{
    public class StringBuilderTextEmailRenderer : IRenderTextEmails
    {
        public string RenderTextBody(Order order)
        {
            var sb = new StringBuilder();
            sb.Append("Hi ").AppendLine(order.CustomerName);
            sb.AppendLine();
            sb.AppendLine("Thanks for ordering from Ray's Music Exchange.");
            sb.AppendLine();
            sb.AppendLine("Your order's being picked and will be shipped as soon as it's ready.");
            sb.AppendLine();
            sb.AppendLine($"Order #: {order.OrderId}");
            sb.AppendLine();
            sb.Append("Track your order at ")
                .AppendLine($"https://raysmusic.exchange/orders/{order.OrderId}");
            sb.AppendLine();
            sb.AppendLine("Order Summary");
            sb.AppendLine();
            sb.AppendLine("Item                                         Qty       Price       Total");
            // Col Widths: <-------------------------44---------------><4-><----12----><----12---->
            sb.AppendLine("------------------------------------------------------------------------");
            foreach (var item in order.Items) sb.AppendOrderItem(item);
            sb.AppendLine("------------------------------------------------------------------------");
            sb.AppendPadRight("Subtotal", 60).AppendPriceGbp(order.Subtotal).AppendLine();
            sb.AppendPadRight("VAT Sales Tax @ 20%", 60).AppendPriceGbp(order.TaxAmount).AppendLine();
            sb.AppendPadRight("Shipping", 60).AppendPriceGbp(order.ShippingCost).AppendLine();
            sb.AppendLine("------------------------------------------------------------------------");
            sb.AppendPadRight("Total", 60).AppendPriceGbp(order.TotalCost).AppendLine();
            return sb.ToString();


        }
    }
}

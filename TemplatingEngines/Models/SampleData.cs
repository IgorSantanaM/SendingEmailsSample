using TemplatingEngines.Common;

namespace TemplatingEngines.Models;

public class SampleData {
	public static class Orders {
		public static Order AB123 = new() {
			OrderId = "AB123",
			CustomerName = "David Evans",
			CustomerEmail = "d.evans@example.com",
			ShippingAddress = new("17 Angel Street", "Harlem", "Umberton", "GB", "U2 1AB"),
			ShippingCost = 12.99m,
			Items = [
				new("Fender Stratocaster US Player Edition", 1, 799m),
				new("VOX AC30 Custom with Celestion Greenback Speakers", 2, 949m),
				new("Dunlop EP103 Echoplex Delay Pedal", 1, 229m),
				new("Ernie Ball Regular Slinky Guitar Strings", 3, 7.99m)
			]
		};

		public static Order CD456 = new() {
			OrderId = "CD456",
			CustomerName = "Victoria Hesketh",
			CustomerEmail = "v.hesketh@example.com",
			ShippingAddress = new("679 Kobalt Street", "Newton", "Yorkpool", "GB", "YP2 4GQ"),
			ShippingCost = 9.99m,
			Items = [
				new("Yamaha Tenori-on", 1, 599m),
				new("Focusrite Scarlett 4i4 4th Gen Audio Interface", 1, 234m),
				new("Arturia BeatStep Pro Hardware Step-Sequencer", 1, 239m),
				new("Arturia MicroFreak Vocoder Microphone", 1, 25m)
			]
		};

		public static Order EF789 = new() {
			OrderId = "EF789",
			CustomerName = "Gordon Sumner",
			CustomerEmail = "gordo@example.com",
			ShippingAddress = new("15 Zenyatta Avenue", "Mondatta", "Buenos Aires", "AR", "C1420"),
			ShippingCost = 55m,
			Items = [
				new("Ibanez UB805-MOB Upright Electric Bass", 1, 999m),
				new("Fender American Vintage II 1960 Precision Bass in 3-Colour Sunburst", 1, 1999m),
				new("Orange Crush Bass 100 Combo", 1, 389m),
				new("Ray's Premium 5m Guitar Lead", 3, 9.99m)
			]
		};

		public static List<Order> AllOrders => [AB123, CD456, EF789];

		public static Order? Find(string id) {
			return id.ToLowerInvariant() switch {
				"ab123" => AB123,
				"cd456" => CD456,
				"ef789" => EF789,
				_ => null
			};
		}
	}
}

using Microsoft.AspNetCore.Mvc;

namespace TemplatingEngines.Controllers;

public class EmailsController : Controller {
	public IActionResult Index() {
		var allOrders = SampleData.Orders.AllOrders;
		return View(allOrders);
	}
}

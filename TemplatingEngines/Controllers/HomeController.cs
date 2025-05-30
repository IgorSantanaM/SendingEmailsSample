using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace TemplatingEngines.Controllers;

public class HomeController : Controller {
	private readonly ILogger<HomeController> logger;

	public HomeController(ILogger<HomeController> logger) {
		this.logger = logger;
	}

	public IActionResult Index() => View();

	[ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
	public IActionResult Error() {
		return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
	}
}

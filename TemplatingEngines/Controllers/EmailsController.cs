using Microsoft.AspNetCore.Mvc;
using System.Text;
using TemplatingEngines.Services;

namespace TemplatingEngines.Controllers;

public class EmailsController(
	StringBuilderTextEmailRenderer sbRenderer,
	FluidEmailRenderer fluid,
    RazorEngineNetCoreEmailRenderer razorEmailRenderer
    ) : Controller {

	
	public IActionResult Index() {
		var allOrders = SampleData.Orders.AllOrders;
		return View(allOrders);
	}

	public IActionResult StringBuilder(string id)
	{
        var order = SampleData.Orders.Find(id);
		if(order == null) return NotFound();
		var body = sbRenderer.RenderTextBody(order);
		return Content(body);
    }

	public IActionResult Fluid(string id)
    {
        var order = SampleData.Orders.Find(id);
        if (order == null) return NotFound();
        var html = fluid.RenderHtmlBody(order);
        return Content(html, "text/html", Encoding.UTF8);
    }
    public IActionResult RazorEngine(string id)
    {
        var order = SampleData.Orders.Find(id);
        if (order == null) return NotFound();
        var html = razorEmailRenderer.RenderHtmlBody(order);
        return Content(html, "text/html", Encoding.UTF8);
    }
}

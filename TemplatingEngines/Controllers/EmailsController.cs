using MailKit.Net.Smtp;
using Microsoft.AspNetCore.Mvc;
using MimeKit;
using System.Text;
using TemplatingEngines.Common;
using TemplatingEngines.Services;

namespace TemplatingEngines.Controllers;

public class EmailsController(
	StringBuilderTextEmailRenderer sbRenderer,
	FluidEmailRenderer fluid,
    RazorEngineNetCoreEmailRenderer razorEmailRenderer,
    RazorComponentHtmlEmailRenderer rchEmailRenderer
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
        => Render(id, fluid.RenderHtmlBody);

    public IActionResult RazorEngine(string id)
        => Render(id, razorEmailRenderer.RenderHtmlBody);

    public IActionResult RazorComponents(string id)
        => Render(id, rchEmailRenderer.RenderHtmlBody);

    private IActionResult Render(string id, Func<Order, string> render)
    {
        var order = SampleData.Orders.Find(id);
        if (order == null) return NotFound();
        var html = render(order);
        return Content(html, "text/html", Encoding.UTF8);
    }

    public async Task<IActionResult> Send(string id, string? bcc, HtmlRendererEnum renderer)
    {
        var order = SampleData.Orders.Find(id);
        if (order == null) return NotFound();
        var message = new MimeMessage();
        message.From.Add(new MailboxAddress("Ray's Music Exchange", "orders@raysmusicexchange"));
        message.To.Add(new MailboxAddress(order.CustomerName, order.CustomerEmail));
        if (bcc != default) message.Bcc.Add(MailboxAddress.Parse(bcc));

        message.Subject = $"Order confirmation {order.OrderId}";

        var textBody = sbRenderer.RenderTextBody(order);
        var htmlBody = renderer switch
        {
            HtmlRendererEnum.Fluid => fluid.RenderHtmlBody(order),
            HtmlRendererEnum.RazorEngine => razorEmailRenderer.RenderHtmlBody(order),
            HtmlRendererEnum.RazorComponent => rchEmailRenderer.RenderHtmlBody(order),
            _ => throw new ArgumentOutOfRangeException(nameof(renderer), renderer, null)
        };

        var bb = new BodyBuilder
        {
            TextBody = textBody,
            HtmlBody = htmlBody
        };

        message.Body = bb.ToMessageBody();
        using var smtp = new SmtpClient();
        await smtp.ConnectAsync("localhost", 1025); 
        await smtp.SendAsync(message);
        await smtp.DisconnectAsync(true);

        return Content($"Mail sent to {order.CustomerEmail}");
    }
}

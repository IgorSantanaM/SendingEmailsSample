using MailKit.Net.Smtp;
using Microsoft.AspNetCore.Mvc;
using MimeKit;
using MimeKit.Utils;
using System.Text;
using TemplatingEngines.Common;
using TemplatingEngines.Services;

namespace TemplatingEngines.Controllers;

public class EmailsController(
	StringBuilderTextEmailRenderer sbRenderer,
	FluidEmailRenderer fluid,
    RazorEngineNetCoreEmailRenderer razorEmailRenderer,
    RazorComponentHtmlEmailRenderer rchEmailRenderer,
    SmtpSettings smtpSettings,
    MjmlEmailRenderer mjml,
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

    public IActionResult Mjml(string id)
        => Render(id, mjml.RenderHtmlBody);

    private IActionResult Render(string id, Func<Order, string> render)
    {
        var order = SampleData.Orders.Find(id);
        if (order == null) return NotFound();
        var html = render(order);
        return Content(html, "text/html", Encoding.UTF8);
    }
    private readonly string[] testEmails = [
        "igorsantanamedeiros17@gmail.com",
        "igorsantanamedeiros@outlook.com"
        ];

    public async Task<IActionResult> Send(string id, string? bcc, HtmlRendererEnum renderer)
    {
        var order = SampleData.Orders.Find(id);
        if (order == null) return NotFound();
        var message = new MimeMessage();
        message.From.Add(new MailboxAddress("Ray's Music Exchange", "orders@raysmusicexchange"));
        message.To.Add(new MailboxAddress(order.CustomerName, order.CustomerEmail));

        message.Cc.AddRange(testEmails.Select(email => MailboxAddress.Parse(email)));

        if (bcc != default) message.Bcc.Add(MailboxAddress.Parse(bcc));

        message.Subject = $"Order confirmation {order.OrderId}";

        var textBody = sbRenderer.RenderTextBody(order);
        var htmlBody = renderer switch
        {
            HtmlRendererEnum.Fluid => fluid.RenderHtmlBody(order),
            HtmlRendererEnum.RazorEngine => razorEmailRenderer.RenderHtmlBody(order),
            HtmlRendererEnum.RazorComponent => rchEmailRenderer.RenderHtmlBody(order),
            HtmlRendererEnum.Mjml => mjml.RenderHtmlBody(order),
            _ => throw new ArgumentOutOfRangeException(nameof(renderer), renderer, null)
        };

        var bb = new BodyBuilder
        {
            TextBody = textBody,
        };

        var logotypeEntity = await bb.Attachments.AddAsync("wwwroot}/rays-music-exchange-logotype-white");
        logotypeEntity.ContentId = MimeUtils.GenerateMessageId("raysmusic.exchange");

        htmlBody = htmlBody.Replace(
            "https://raysmusic.exchange/images/rays-music-exchange-logotype-white.png",
            $"cid:{logotypeEntity.ContentId}");

        bb.HtmlBody = htmlBody;

        message.Body = bb.ToMessageBody();
        using var smtp = new SmtpClient();
        await smtp.ConnectAsync(smtpSettings.Host, smtpSettings.Port); 
        if(smtpSettings.UserName != null)
        {
            await smtp.AuthenticateAsync(smtpSettings.UserName, smtpSettings.Password);
        }
        await smtp.SendAsync(message);
        await smtp.DisconnectAsync(true);

        return Content($"Mail sent to {order.CustomerEmail}");
    }

}

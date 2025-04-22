using Mjml.Net;
using System.Security.Cryptography.X509Certificates;
using TemplatingEngines.Common;

namespace TemplatingEngines.Services;

public class MjmlEmailRenderer(MjmlRenderer mjml) : IRenderHtmlEmails
{

    public string RenderHtmlBody(Order order)
         => RenderHtmlBodyAsync(order).Result;
    public async Task<string> RenderHtmlBodyAsync(Order order)
    {
        var mjmlSource = File.ReadAllText("Templates/Emails/OrderEmail.mjml");
        var options = new MjmlOptions
        {
            PostProcessors = [AngleSharpPostProcessor.Default]
        };
        mjml.AddHtmlAttributes();
        var (output, errors) = await mjml.RenderAsync(mjmlSource, options);
        if (errors.Any()) throw new(errors.First().Error);
        return output;
    }
}
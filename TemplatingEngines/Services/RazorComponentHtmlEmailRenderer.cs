using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using RazorEmailComponent;
using TemplatingEngines.Common;

namespace TemplatingEngines.Services;

public class RazorComponentHtmlEmailRenderer(IServiceProvider serviceProvider) : IRenderHtmlEmails
{
    public async Task<string> RenderHtmlBodyAsync(Order order)
    {
        var loggerFactory = serviceProvider.GetRequiredService<ILoggerFactory>();
        await using var htmlRenderer = new HtmlRenderer(serviceProvider, loggerFactory);
        return await htmlRenderer.Dispatcher.InvokeAsync(async () =>
        {
            var dictionary = new Dictionary<string, object?>
            {
                { "Order", order }
            };

            var parameters = ParameterView.FromDictionary(dictionary);
            var output = await htmlRenderer.RenderComponentAsync<OrderEmail>(parameters);
            return output.ToHtmlString();
        });

    }
    public string RenderHtmlBody(Order order) => RenderHtmlBodyAsync(order).Result; 
}

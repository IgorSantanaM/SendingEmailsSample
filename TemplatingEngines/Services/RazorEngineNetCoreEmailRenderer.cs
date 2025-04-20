using RazorEngine.Templating;
using TemplatingEngines.Common;

namespace TemplatingEngines.Services;

public class RazorEngineNetCoreEmailRenderer(IRazorEngineService engine) : IRenderHtmlEmails
{
    private const string TEMPLATE_PATH = "Templates/Emails/OrderEmail.cshtml";
    public string RenderHtmlBody(Order order)
    {
        if (!engine.IsTemplateCached(TEMPLATE_PATH, typeof(Order))) CacheTemplate();
        return engine.Run(TEMPLATE_PATH, typeof(Order), order);
    }

    public void CacheTemplate()
    {
        engine.AddTemplate(TEMPLATE_PATH, File.ReadAllText(TEMPLATE_PATH));
        engine.Compile(TEMPLATE_PATH, typeof(Order));
    }       
}
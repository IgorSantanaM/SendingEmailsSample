using Mjml.Net;
using RazorEngine.Templating;
using TemplatingEngines.Common;

namespace TemplatingEngines.Services;

public class MjmlEmailRenderer(MjmlRenderer mjml,
    IRazorEngineService razor
    ) : IRenderHtmlEmails
{
    private const string TEMPLATE_PATH = "Templates/Emails/OrderEmail.cs.mjml";

    private readonly string[] cssAtRules = [
        "bottom-center", "bottom-left", "bottom-left-corner", "bottom-right", "bottom-right-corner", "charset", "counter-style",
        "document", "font-face", "font-feature-values", "import", "left-bottom", "left-middle", "left-top", "keyframes", "media",
        "namespace", "page", "right-bottom", "right-middle", "right-top", "supports", "top-center", "top-left", "top-left-corner",
        "top-right", "top-right-corner"
    ];

    /// <summary>
    /// Replace each of the cssAtRules that starts with @ and replace to @@ so the razor engine don't get confused
    /// </summary>
    /// <param name="razor"></param>
    /// <returns></returns>
    private string EscapeCssRulesInRazorTemplate(string mjmlOutput) =>
        cssAtRules.Aggregate(mjmlOutput,
            (current, rule) => current.Replace($"@{rule}", $"@@{rule}"));
    /// <summary>
    /// Transforms the MJML to HTML so the Razor Engine can compile it.
    /// </summary>
    public void CacheTemplate()
    {
        var mjmlSource = File.ReadAllText(TEMPLATE_PATH);
        var options = new MjmlOptions();
        var (mjmlOutput, errors) = mjml.Render(mjmlSource, options);
        if (errors.Any()) throw new(errors.First().Error);
        mjmlOutput = EscapeCssRulesInRazorTemplate(mjmlOutput);
        mjmlOutput = mjmlOutput.Replace("wght@0", "wght@@0");
        razor.AddTemplate(TEMPLATE_PATH, mjmlOutput);
        razor.Compile(TEMPLATE_PATH, typeof(Order));
    }

    public string RenderHtmlBody(Order order)
    {
        if (!razor.IsTemplateCached(TEMPLATE_PATH, typeof(Order))) CacheTemplate();
        return razor.Run(TEMPLATE_PATH, typeof(Order), order);
    }
}
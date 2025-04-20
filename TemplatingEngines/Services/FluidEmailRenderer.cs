using Fluid;
using TemplatingEngines.Common;

namespace TemplatingEngines.Services;

public class FluidEmailRenderer : IRenderHtmlEmails
{
    private static readonly FluidParser parser = new();
    private IFluidTemplate? template;
    public static readonly TemplateOptions options = new();
    static FluidEmailRenderer()
    {
        options.MemberAccessStrategy.Register<OrderItem>();
        options.MemberAccessStrategy.Register<Address>();
        options.MemberAccessStrategy.Register<Order>();

        // Or if security is not a consideration, you can do this: 
        //options.MemberAccessStrategy = new UnsafeMemberAccessStrategy();
    }

    static IFluidTemplate ParseTemplate(string path)
    {
        var liquid = File.ReadAllText(path);
        return parser.Parse(liquid);
    }

    public string RenderHtmlBody(Order order)
    {
        template ??= ParseTemplate("Templates/Emails/OrderEmail.liquid.html");

        var context = new TemplateContext(order, options);
        return template.Render(context);
    }
}
using TemplatingEngines.Common;

namespace TemplatingEngines.Services;

public interface IRenderHtmlEmails {
	string RenderHtmlBody(Order order);
}

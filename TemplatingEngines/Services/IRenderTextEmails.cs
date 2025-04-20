using TemplatingEngines.Common;

namespace TemplatingEngines.Services;

public interface IRenderTextEmails {
	string RenderTextBody(Order order);
}

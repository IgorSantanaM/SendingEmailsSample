namespace TemplatingEngines.Services;

public class SmtpSettings
{
	public string Host { get; set; } = "localhost";
	public int Port { get; set; } = 1025;
	public string? UserName { get; set; }
    public string? Password { get; set; }	
}

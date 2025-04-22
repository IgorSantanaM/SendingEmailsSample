using Mjml.Net;
using RazorEngine.Templating;
using TemplatingEngines.Services;

var builder = WebApplication.CreateBuilder(args);

var smtpSettings = builder.Configuration.GetRequiredSection("Smtp").Get<SmtpSettings>()
	?? throw new Exception("Smtp settings not found in configuration.");

var services = builder.Services;

services.AddSingleton(smtpSettings);

// Add services to the container.
services.AddControllersWithViews();

services.AddSingleton<StringBuilderTextEmailRenderer>();
services.AddTransient<MjmlRenderer>();
services.AddTransient<MjmlEmailRenderer>();
 
#if DEBUG
services.AddTransient<FluidEmailRenderer>();
#else
services.AddSingleton<FluidEmailRenderer>();
#endif
services.AddTransient<RazorComponentHtmlEmailRenderer>();

services.AddTransient(_ => RazorEngineService.Create());
services.AddTransient<RazorEngineNetCoreEmailRenderer>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment()) {
	app.UseExceptionHandler("/Home/Error");
	// The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
	app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
	name: "default",
	pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
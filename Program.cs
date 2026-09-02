using TelemetryDeviceV1.Setup;



WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

builder.RegisterServices();

WebApplication app = builder.Build();

app.MapControllers();

app.Run();
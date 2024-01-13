global using FastEndpoints;
using FastEndpoints.Swagger;
var builder = WebApplication.CreateBuilder(args);

builder.Logging.AddSimpleConsole(options =>
{
    options.TimestampFormat = "hh:mm:ss ";
});

builder.Services.AddFastEndpoints();

builder.Services.SwaggerDocument(o =>
{
    o.DocumentSettings = s =>
    {
        s.DocumentName = "swagger";
        s.Title = "Restaurant FIAP";
        s.Version = "v1";
        s.Description = "Documentation about endpoints";
    };

    o.EnableJWTBearerAuth = false;
    o.ShortSchemaNames = false;
    o.RemoveEmptyRequestSchema = true;
});

var app = builder.Build();
app.Run();
global using FastEndpoints;
using Api;
using Domain.Ports;
using Domain.Services;
using FastEndpoints.Swagger;
using Infrastructure.PaymentGateway;
using Infrastructure.PaymentGateway.Webhook;
using Infrastructure.PostgreDb;
using Infrastructure.PostgreDb.Repository;
using Infrastructure.RabbitMq;

var builder = WebApplication.CreateBuilder(args);

builder.Logging.AddSimpleConsole(options =>
{
    options.TimestampFormat = "hh:mm:ss ";
});

builder.Services.AddFastEndpoints();
builder.Services.AddHealthChecks();

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

builder.Services.AddHttpClient();

// ** CONTEXT POSTGRE**
var postgreDbSettings = builder.Configuration.GetSection("PostgreDbSettings").Get<PostgreDbSettings>();

builder.Services.AddSingleton<Context>
    (
    sp => new Context
                       (
                        postgreDbSettings!.PostgresConnection
                        ));

// ** RabbitMQ **
builder.Services.AddSingleton<IRabbitMqSettings>(_ => builder.Configuration.GetSection("RabbitMqSettings").Get<RabbitMqSettings>());

// ** SERVICE **
builder.Services.AddScoped<IClientUseCase, ClientUseCase>();
builder.Services.AddScoped<IItemMenuUseCase, ItemMenuUseCase>();
builder.Services.AddScoped<IOrderUseCase, OrderUseCase>();
builder.Services.AddScoped<IPaymentGateway, PaymentGateway>();
builder.Services.AddScoped<IOrderQueue, OrderQueue>();
builder.Services.AddScoped<IPaymentWebHook, PaymentWebHook>();

// ** REPOSITORY **
builder.Services.AddScoped<IClientGateway, ClientGateway>();
builder.Services.AddScoped<IItemMenuGateway, ItemMenuGateway>();
builder.Services.AddScoped<IOrderGateway, OrderGateway>();

var app = builder.Build();

app.UseFastEndpoints(c =>
{
    c.Endpoints.ShortNames = false;

    c.Endpoints.Configurator = ep =>
    {
        ep.Summary(s =>
        {
            s.Response<ErrorResponse>(400);
            s.Response(401);
            s.Response(403);
            s.Responses[200] = "OK";
        });

        ep.PostProcessors(FastEndpoints.Order.After, new GlobalLoggerPostProcces
        (
            LoggerFactory.Create(builder =>
            {
                builder.AddConsole();
            }).CreateLogger<GlobalLoggerPostProcces>()
        ));
    };
}).UseSwaggerGen();

app.Run();
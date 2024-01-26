global using FastEndpoints;
using Api;
using Domain.Ports;
using Domain.Services;
using FastEndpoints.Swagger;
using Infrastructure.MongoDb;
using Infrastructure.MongoDb.Repository;
using Infrastructure.PaymentGateway;
using Infrastructure.RabbitMq;
using RabbitMQ.Client;

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

builder.Services.AddHttpClient();

// ** CONTEXT MONGODB**
var mongoDbSettings = builder.Configuration.GetSection("MongoDbSettings").Get<MongoDbSettings>();

builder.Services.AddSingleton<Context>
    (
    sp => new Context
                       (
                        mongoDbSettings!.ConnectionString,
                        mongoDbSettings!.DatabaseName
                        ));

// ** RabbitMQ **
var rabbitMqSettings = builder.Configuration.GetSection("RabbitMqSettings").Get<RabbitMqSettings>();

builder.Services.AddSingleton(sp =>
{
    var factory = new ConnectionFactory
    {
        HostName = rabbitMqSettings!.HostName,
        UserName = rabbitMqSettings!.UserName,
        Password = rabbitMqSettings!.Password,
        Port = rabbitMqSettings!.Port,
        DispatchConsumersAsync = true
    };
    return factory.CreateConnection();
});

builder.Services.AddScoped<IModel>(sp =>
{
    var connection = sp.GetRequiredService<IConnection>();
    return connection.CreateModel();
});

// ** SERVICE **
builder.Services.AddScoped<IClientService, ClientService>();
builder.Services.AddScoped<IItemMenuService, ItemMenuService>();
builder.Services.AddScoped<IOrderService, OrderService>();
builder.Services.AddScoped<IPaymentService, PaymentService>();
builder.Services.AddScoped<IOrderQueue, OrderQueue>();

// ** REPOSITORY **
builder.Services.AddScoped<IClientRepository, ClientRepository>();
builder.Services.AddScoped<IItemMenuRepository, ItemMenuRepository>();
builder.Services.AddScoped<IOrderRepository, OrderRepository>();

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
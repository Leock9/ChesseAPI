using FluentValidation.Results;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text;

namespace Api;

public class GlobalLoggerPostProcces(ILogger<GlobalLoggerPostProcces> logger) : IGlobalPostProcessor
{
    private readonly ILogger<GlobalLoggerPostProcces> _logger = logger;

    public Task PostProcessAsync(object req, object? res, HttpContext ctx, IReadOnlyCollection<ValidationFailure> failures, CancellationToken ct)
    {
        if (req is not null)
        {
            var hearder = new StringBuilder($"Request {DateTime.Now}").AppendLine();
            hearder.AppendLine($"Enpoint:{ctx.Request.Path}");
            hearder.AppendLine($"Host:{ctx.Request.Headers.Host}");
            hearder.AppendLine($"Method:{ctx.Request.Method}");

            var token = ctx.Request.Headers.Authorization.ToString();
            token = $"{token[..10]}...{token[^3..]}";
            hearder.AppendLine($"Token:{token}");

            var jsonBody = JsonSerializer.Serialize(req, new JsonSerializerOptions
            {
                WriteIndented = true,
                Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping
            });

            hearder.AppendLine($"Request Data:{jsonBody}");

            _logger.LogInformation("{header}", hearder);

            if (failures.Any())
            {
                var validation = new StringBuilder();
                foreach (var validationFailure in failures) { validation.AppendLine(validationFailure.ToString()); }

                _logger.LogInformation("Validações respondidas:{validation}", validation.ToString());
            }
        }

        if (res is not null)
        {
            _logger.LogInformation
                    (
                        "Endpoint retornou StatusCode:{ctx.Response?.StatusCode} Mensagem:{res}",
                        ctx.Response?.StatusCode,
                        JsonSerializer.Serialize(res, new JsonSerializerOptions
                        {
                            WriteIndented = true,
                            Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping
                        })
                    );
        }

        return Task.CompletedTask;
    }

    public Task PostProcessAsync(IPostProcessorContext context, CancellationToken ct)
    {
        if (context.Request is not null)
        {
            var hearder = new StringBuilder($"Request {DateTime.Now}").AppendLine();
            hearder.AppendLine($"Enpoint:{context.HttpContext.Request.Path}");
            hearder.AppendLine($"Host:{context.HttpContext.Request.Headers.Host}");
            hearder.AppendLine($"Method:{context.HttpContext.Request.Method}");

            var token = context.HttpContext.Request.Headers.Authorization.ToString();
            token = $"{token[..10]}...{token[^3..]}";
            hearder.AppendLine($"Token:{token}");

            var jsonBody = JsonSerializer.Serialize(context.Request, new JsonSerializerOptions
            {
                WriteIndented = true,
                Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping
            });

            hearder.AppendLine($"Request Data:{jsonBody}");

            _logger.LogInformation("{header}", hearder);

            if (context.ValidationFailures.Any())
            {
                var validation = new StringBuilder();
                foreach (var validationFailure in context.ValidationFailures) { validation.AppendLine(validationFailure.ToString()); }

                _logger.LogInformation("Validações respondidas:{validation}", validation.ToString());
            }
        }

        if (context.Response is not null)
        {
            _logger.LogInformation
                    (
                        "Endpoint retornou StatusCode:{ctx.Response?.StatusCode} Mensagem:{res}",
                        context.HttpContext.Response?.StatusCode,
                        JsonSerializer.Serialize(context.HttpContext.Response, new JsonSerializerOptions
                        {
                            WriteIndented = true,
                            Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping
                        })
                    );
        }

        return Task.CompletedTask;
    }
}

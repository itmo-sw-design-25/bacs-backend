namespace BaCS.Presentation.API.Swagger.Filters;

using System.Net.Mime;
using System.Reflection;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

public class ProblemDetailsOperationFilter : IOperationFilter
{
    private static readonly Dictionary<string, Func<OpenApiExample>> ProblemDetailExamples = new()
    {
        {
            "400", () => new OpenApiExample
            {
                Value = new OpenApiObject
                {
                    ["type"] = new OpenApiString("https://tools.ietf.org/html/rfc7231#section-6.5.1"),
                    ["title"] = new OpenApiString(ReasonPhrases.GetReasonPhrase(400)),
                    ["status"] = new OpenApiInteger(400)
                },
                Description = ReasonPhrases.GetReasonPhrase(400),
            }
        },
        {
            "401", () => new OpenApiExample
            {
                Value = new OpenApiObject
                {
                    ["type"] = new OpenApiString("https://tools.ietf.org/html/rfc7235#section-3.1"),
                    ["title"] = new OpenApiString(ReasonPhrases.GetReasonPhrase(401)),
                    ["status"] = new OpenApiInteger(401)
                },
                Description = ReasonPhrases.GetReasonPhrase(401),
            }
        },
        {
            "403", () => new OpenApiExample
            {
                Value = new OpenApiObject
                {
                    ["type"] = new OpenApiString("https://tools.ietf.org/html/rfc7231#section-6.5.3"),
                    ["title"] = new OpenApiString(ReasonPhrases.GetReasonPhrase(403)),
                    ["status"] = new OpenApiInteger(403)
                },
                Description = ReasonPhrases.GetReasonPhrase(403),
            }
        },
        {
            "404", () => new OpenApiExample
            {
                Value = new OpenApiObject
                {
                    ["type"] = new OpenApiString("https://tools.ietf.org/html/rfc7231#section-6.5.4"),
                    ["title"] = new OpenApiString(ReasonPhrases.GetReasonPhrase(404)),
                    ["status"] = new OpenApiInteger(404)
                },
                Description = ReasonPhrases.GetReasonPhrase(404),
            }
        },
        {
            "500", () => new OpenApiExample
            {
                Value = new OpenApiObject
                {
                    ["type"] = new OpenApiString("https://tools.ietf.org/html/rfc7231#section-6.6.1"),
                    ["title"] = new OpenApiString(ReasonPhrases.GetReasonPhrase(500)),
                    ["status"] = new OpenApiInteger(500)
                },
                Description = ReasonPhrases.GetReasonPhrase(500),
            }
        }
    };

    public void Apply(OpenApiOperation operation, OperationFilterContext context)
    {
        var apiDescription = context.ApiDescription;

        foreach (var response in operation.Responses)
        {
            if (!ProblemDetailExamples.TryGetValue(response.Key, out var problemDetailFactory)) continue;
            if (!response.Value.Content.TryGetValue(MediaTypeNames.Application.ProblemJson, out var error)) continue;

            var problemDetailExample = problemDetailFactory();

            if (problemDetailExample.Value as OpenApiObject is not { } problemDetailsObject) continue;

            problemDetailsObject["instance"] =
                new OpenApiString($"{apiDescription.HttpMethod} /{apiDescription.RelativePath}");

            if (response.Key == "400") EnrichWithValidationErrors(problemDetailsObject, apiDescription);

            error.Examples = new Dictionary<string, OpenApiExample>
            {
                { problemDetailExample.Description, problemDetailExample }
            };
        }
    }

    private static void EnrichWithValidationErrors(OpenApiObject problemDetailObject, ApiDescription apiDescription)
    {
        var errorsObject = new OpenApiObject();

        foreach (var parameter in apiDescription.ParameterDescriptions.Where(x => x.Source != BindingSource.Path))
        {
            if (parameter.Source == BindingSource.Body)
            {
                var type = parameter.Type;
                var properties = type.GetProperties(BindingFlags.Public | BindingFlags.Instance);

                foreach (var property in properties.Select(p => p.Name))
                {
                    errorsObject.Add(
                        property,
                        new OpenApiArray { new OpenApiString($"The '{property}' field is not valid") }
                    );
                }
            }
            else if (parameter.IsRequired)
            {
                errorsObject.Add(
                    parameter.Name,
                    new OpenApiArray { new OpenApiString($"The '{parameter.Name}' field is required") }
                );
            }
            else
            {
                errorsObject.Add(
                    parameter.Name,
                    new OpenApiArray { new OpenApiString($"The '{parameter.Name}' field is not valid") }
                );
            }
        }

        if (errorsObject is not { Count: > 0 }) return;

        problemDetailObject.Add("errors", errorsObject);
    }
}

namespace BaCS.Presentation.API.Swagger.SchemaFilters;

using Microsoft.AspNetCore.Mvc;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

public class ProblemDetailsSchemaFilter : ISchemaFilter
{
    public void Apply(OpenApiSchema schema, SchemaFilterContext context)
    {
        if (context.Type != typeof(ProblemDetails)) return;

        schema.Description =
            """
            Машино-читаемый формат ошибок API, в соответствии со стандартом <a href="https://www.rfc-editor.org/rfc/rfc9457">RFC 9457</a>.
            """;

        schema.Properties["type"].Description = "Тип ошибки.";
        schema.Properties["title"].Description = "Краткое человеко-читаемое описание ошибки.";
        schema.Properties["status"].Description = "HTTP-статус код ошибки.";
        schema.Properties["detail"].Description = "Детальное человеко-читаемое описание ошибки.";
        schema.Properties["instance"].Description = "URI, на котором произошла ошибка.";
    }
}

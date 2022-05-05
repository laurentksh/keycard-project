using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace KeyCardWebServices.Auth;

public class CustomHeaderOperationFilter : IOperationFilter
{
    private readonly string _headerName;
    private readonly string _headerDescription;

    public CustomHeaderOperationFilter(string headerName, string headerDescription)
    {
        _headerName = headerName;
        _headerDescription = headerDescription;
    }

    public void Apply(OpenApiOperation operation, OperationFilterContext context)
    {
        if (operation.Parameters == null)
            operation.Parameters = new List<OpenApiParameter>();

        operation.Parameters.Add(new OpenApiParameter
        {
            Name = _headerName,
            In = ParameterLocation.Header,
            
            Required = false,
            Description = _headerDescription,

            Schema = new OpenApiSchema
            {
                Type = "string"
            }
        });
    }
}

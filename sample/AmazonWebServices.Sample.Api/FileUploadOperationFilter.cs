namespace AmazonWebServices.Sample.Api;

// ReSharper disable once ClassNeverInstantiated.Global
public class FileUploadOperationFilter : IOperationFilter
{
    /// <summary>
    /// Provides upload control for objects of type 'IFromFile' in the Swagger interface.
    /// </summary>
    public void Apply(OpenApiOperation operation, OperationFilterContext context)
    {
        if (RequestBodyControl(operation.RequestBody))
            return;

        operation.RequestBody.Content["multipart/form-data"].Schema.Properties = context.MethodInfo
            .GetParameters()
            .Where(p => p.ParameterType == typeof(IFormFile))
            .ToDictionary(k => k.Name, _ => new OpenApiSchema
            {
                Type = "string",
                Format = "binary"
            });
    }

    private static bool RequestBodyControl(
        OpenApiRequestBody requestBody) =>
        requestBody == null || !requestBody.Content.Any(x => x.Key.Equals("multipart/form-data", StringComparison.InvariantCultureIgnoreCase));
}
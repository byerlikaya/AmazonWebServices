using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace AmazonWebServices.Sample.Api
{
    public class FileUploadOperationFilter : IOperationFilter
    {
        /// <summary>
        /// Provides upload control for objects of type 'IFromFile' in the Swagger interface.
        /// </summary>
        public void Apply(OpenApiOperation operation, OperationFilterContext context)
        {
            const string fileUploadMime = "multipart/form-data";
            if (operation.RequestBody == null || !operation.RequestBody.Content.Any(x => x.Key.Equals(fileUploadMime, StringComparison.InvariantCultureIgnoreCase)))
                return;

            var fileParams = context.MethodInfo.GetParameters().Where(p => p.ParameterType == typeof(IFormFile));
            operation.RequestBody.Content[fileUploadMime].Schema.Properties =
                fileParams.ToDictionary(k => k.Name, _ => new OpenApiSchema()
                {
                    Type = "string",
                    Format = "binary"
                });
        }
    }
}

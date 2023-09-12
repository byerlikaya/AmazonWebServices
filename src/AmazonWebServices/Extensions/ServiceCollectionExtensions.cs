using AmazonWebServices.Interfaces;
using AmazonWebServices.Services;
using AmazonWebServices.Utilities;
using Microsoft.Extensions.DependencyInjection;

namespace AmazonWebServices.Extensions
{
    internal static class ServiceCollectionExtensions
    {
        public static void AddAmazonWebServices(this IServiceCollection services)
        {
            services.AddSingleton<IFileService, AmazonS3Service>();
            ServiceTool.Create(services);
        }
    }
}

using AmazonWebServices.Interfaces;
using AmazonWebServices.Services;
using AmazonWebServices.Utilities;
using Microsoft.Extensions.DependencyInjection;

namespace AmazonWebServices.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static void AddAmazonWebServices(this IServiceCollection services)
        {
            services.AddSingleton<IAmazonS3Service, AmazonS3Service>();
            services.AddSingleton<IAmazonSesService, AmazonSesService>();
            ServiceTool.Create(services);
        }
    }
}

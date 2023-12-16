namespace AmazonWebServices.Extensions;

public static class ServiceCollectionExtensions
{
    public static void AddAmazonWebServices(this IServiceCollection services)
    {
        services.AddScoped<IAmazonS3Service, AmazonS3Service>();
        services.AddSingleton<IAmazonSesService, AmazonSesService>();
        ServiceTool.Create(services);
    }
}
namespace AmazonWebServices.Utilities;

internal class ServiceTool
{
    public static IServiceProvider ServiceProvider { get; set; }

    public static IServiceCollection Create(IServiceCollection services)
    {
        ServiceProvider = services.BuildServiceProvider();
        return services;
    }
}
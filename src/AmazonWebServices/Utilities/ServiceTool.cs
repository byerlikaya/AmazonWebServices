namespace AmazonWebServices.Utilities;

// ReSharper disable once ClassNeverInstantiated.Global
internal class ServiceTool
{
    public static IServiceProvider ServiceProvider { get; set; }

    public static IServiceCollection Create(IServiceCollection services)
    {
        ServiceProvider = services.BuildServiceProvider();
        return services;
    }
}
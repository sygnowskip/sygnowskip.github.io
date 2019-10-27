using Common.Docker.Resources;
using Microsoft.Extensions.DependencyInjection;

namespace Common.Docker
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddDockerSetup(this IServiceCollection serviceCollection)
        {
            serviceCollection.AddTransient<MsSqlResourceAwaiter>();
            return serviceCollection;
        }
    }
}
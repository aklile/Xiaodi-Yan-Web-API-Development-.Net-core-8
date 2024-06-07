using MyFirstApi.Models;

namespace MyFirstApi.Extensions
{
    public static class OptionsCollectionExtensions
    {
        public static IServiceCollection AddConfigigration(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<DatabaseOptions>(DatabaseOptions.
             SystemDatabaseSectionName, configuration.
             GetSection($"{DatabaseOptions.SectionName}:{DatabaseOptions.
             SystemDatabaseSectionName}"));


            services.Configure<DatabaseOptions>(DatabaseOptions.
             BusinessDatabaseSectionName, configuration.
             GetSection($"{DatabaseOptions.SectionName}:{DatabaseOptions.
             BusinessDatabaseSectionName}"));

            return services;
        }
    }
}

using Microsoft.Extensions.DependencyInjection;
using WeatherReportNotification.Email;
using WeatherReportNotification.User;

namespace WeatherReportNotification.IoC
{
    public static class DependencyResolver
    {
        public static void ConfigureServices(IServiceCollection services)
        {
            services.AddScoped<IEmailRepository, EmailRepository>()
                .AddScoped<IUserRepository, UserRepository>();
        }
    }
}

using Microsoft.Extensions.DependencyInjection;
using WeatherReportNotification.User;
using WeatherReportNotification.IoC;

class Program
{
    static async Task Main(string[] args)
    {
        var serviceCollection = new ServiceCollection();
        DependencyResolver.ConfigureServices(serviceCollection);
        var serviceProvider = serviceCollection.BuildServiceProvider();
        //mudar nomenclatura do método
        var users = serviceProvider.GetService<IUserRepository>();
        users.GetWeatherReport();
    }
}
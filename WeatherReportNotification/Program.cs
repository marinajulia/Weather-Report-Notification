using Microsoft.Extensions.DependencyInjection;
using RabbitMQ.Client.Events;
using RabbitMQ.Client;
using System.Collections.Generic;
using WeatherReportNotification.Email;
using WeatherReportNotification.Entites;
using WeatherReportNotification.User;
using System.Text;
using Newtonsoft.Json;

class Program
{
    static async Task Main(string[] args)
    {
        var serviceCollection = new ServiceCollection();
        ConfigureServices(serviceCollection);
        var serviceProvider = serviceCollection.BuildServiceProvider();

        var eventService = serviceProvider.GetService<IEmailRepository>();
        var users = serviceProvider.GetService<IUserRepository>();

        var factory = new ConnectionFactory() { HostName = "localhost" };

        using (var connection = factory.CreateConnection())
        using (var channel = connection.CreateModel())
        {
            channel.QueueDeclare(queue: "WeatherReport",
                                 durable: false,
                                 exclusive: false,
                                 autoDelete: false,
                                 arguments: null);

            var consumer = new EventingBasicConsumer(channel);

            consumer.Received += (model, ea) =>
            {
                var body = ea.Body.ToArray();
                string receivedMessage = Encoding.UTF8.GetString(body);
                var deserializedUser = JsonConvert.DeserializeObject<UserWeather>(receivedMessage);
            };

            channel.BasicConsume(queue: "WeatherReport",
                                 autoAck: true,
                                 consumer: consumer);
            Console.ReadLine();
        }

    }

    public static void ConfigureServices(IServiceCollection services)
    {
        services.AddScoped<IEmailRepository, EmailRepository>()
            .AddScoped<IUserRepository, UserRepository>();
    }
}
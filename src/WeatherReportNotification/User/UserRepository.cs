using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;
using WeatherReportNotification.Email;
using WeatherReportNotification.Entites;
using WeatherReportNotification.IoC;

namespace WeatherReportNotification.User
{
    public class UserRepository : IUserRepository
    {
        public void GetWeatherReport()
        {
            var serviceCollection = new ServiceCollection();
            DependencyResolver.ConfigureServices(serviceCollection);
            var serviceProvider = serviceCollection.BuildServiceProvider();
            var eventService = serviceProvider.GetService<IEmailRepository>();

            var factory = new ConnectionFactory() { HostName = "localhost" };

            using (var connection = factory.CreateConnection())
            using (var channel = connection.CreateModel())
            {
                List<UserWeather> receivedMessages = new List<UserWeather>();

                channel.QueueDeclare(queue: "WeatherReport",
                                     durable: false,
                                     exclusive: false,
                                     autoDelete: false,
                                     arguments: null);

                var consumer = new EventingBasicConsumer(channel);

                consumer.Received += (model, ea) =>
                {
                    var body = ea.Body.ToArray();
                    var message = Encoding.UTF8.GetString(body);
                    Console.WriteLine($"[x] Recebida: {message}");

                    var userWeather = JsonConvert.DeserializeObject<UserWeather>(message);
                    receivedMessages.Add(userWeather);
                    eventService.SendEmail(userWeather);
                };

                channel.BasicConsume(queue: "WeatherReport",
                                     autoAck: true,
                                     consumer: consumer);


                Console.ReadLine();
            }
        }
    }
}


using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;
using WeatherReportNotification.Entites;

namespace WeatherReportNotification.User
{
    public class UserRepository : IUserRepository
    {
        public List<UserWeather> GetWeatherReport()
        {
            var factory = new ConnectionFactory() { HostName = "localhost" };
            var receivedMessages = new List<UserWeather>();

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
                    var message = Encoding.UTF8.GetString(body);
                    Console.WriteLine($"[x] Recebida: {message}");

                    var userWeather = JsonConvert.DeserializeObject<UserWeather>(message);
                    receivedMessages.Add(userWeather);
                };

                channel.BasicConsume(queue: "WeatherReport",
                                     autoAck: true,
                                     consumer: consumer);
            }

            return receivedMessages;
        }
    }
}

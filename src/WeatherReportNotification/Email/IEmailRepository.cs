using WeatherReportNotification.Entites;

namespace WeatherReportNotification.Email
{
    public interface IEmailRepository
    {
        public void SendEmail(UserWeather userWeather);
    }
}

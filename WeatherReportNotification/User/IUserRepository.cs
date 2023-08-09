using WeatherReportNotification.Entites;

namespace WeatherReportNotification.User
{
    public interface IUserRepository
    {
        List<UserWeather> GetWeatherReport();
    }
}

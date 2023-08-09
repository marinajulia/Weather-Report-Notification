namespace WeatherReportNotification.Entites
{
    public class UserWeather
    {
        public string Name { get; set; }
        public string Email { get; set; }
        public WeatherReportEntity? WeatherReport { get; set; }
    }
}

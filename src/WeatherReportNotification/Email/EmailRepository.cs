
using System.Net;
using System.Net.Mail;
using WeatherReportNotification.Entites;

namespace WeatherReportNotification.Email
{
    public class EmailRepository : IEmailRepository
    {
        public void SendEmail(UserWeather userWeather)
        {
            //colocar em variaveis de ambiente
            string senderEmail = "estudosmarinajulia707@gmail.com";
            string senderPassword = "ciujlzvkaosmroaz";
            string smtpServer = "smtp.gmail.com";
            int smtpPort = 587;

            MailMessage mailMessage = new MailMessage(senderEmail, userWeather.Email);
            mailMessage.Subject = $"Your weather report for {GetDay()}!";
            mailMessage.Body = $"Good morning {userWeather.Name}!  " +
                $"\n  the forecast for the city of {userWeather.WeatherReport.Cidade}-{userWeather.WeatherReport.estado} {userWeather.WeatherReport.clima[0].data} is: \n" +
                $"Minimum temperature of {userWeather.WeatherReport.clima[0].min}ºC \n" +
                $"Maximum temperature of {userWeather.WeatherReport.clima[0].max}ºC";

            SmtpClient smtpClient = new SmtpClient(smtpServer);
            smtpClient.Port = smtpPort;
            smtpClient.Credentials = new NetworkCredential(senderEmail, senderPassword);
            smtpClient.EnableSsl = true;

            try
            {
                smtpClient.Send(mailMessage);
                Console.WriteLine("E-mail enviado com sucesso!");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ocorreu um erro ao enviar o e-mail: {ex.Message}");
            }
        }

        private string GetDay()
        {
            var day = DateTime.Now.DayOfWeek;
            return day.ToString();
        }
    }
}

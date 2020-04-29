using System.Threading.Tasks;
using SendGrid;

namespace Tayra.Mailer
{
    public interface IEmailService
    {
        Response SendEmail(string sender, string recipient, string subject, string body);
    }
}

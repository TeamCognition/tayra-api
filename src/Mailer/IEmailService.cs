using System.Threading.Tasks;

namespace Tayra.Mailer
{
    public interface IEmailService
    {
        Task Send(string sender, string recipient, string subject, string body);
    }
}

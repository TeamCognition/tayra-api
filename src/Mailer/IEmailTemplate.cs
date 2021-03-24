namespace Tayra.Mailer
{
    public interface IEmailTemplate 
    {
        public string Subject { get; set; }
        public string EmailTemplateFileName { get; }
    }
}
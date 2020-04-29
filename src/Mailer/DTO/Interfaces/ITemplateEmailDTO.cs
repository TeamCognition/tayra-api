namespace Tayra.Mailer
{
    public interface ITemplateEmailDTO
    {
        string TemplateId { get; }
        object TemplateData { get; set; }
    }
}

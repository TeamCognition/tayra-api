using Tayra.Common;

namespace Tayra.Mailer
{
    public class EmailGiftReceivedDTO : ITemplateEmailDTO
    {
        public string TemplateId { get => "d-2fce25d657004f1f861a38e50823427f"; }
        public object TemplateData { get; set; }

        public EmailGiftReceivedDTO(string senderFullName, ItemTypes itemType)
        {
            TemplateData = new
            {
                FullName = senderFullName,
                ItemType = itemType
            };
        }
    }
}

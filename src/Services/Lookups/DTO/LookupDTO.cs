namespace Tayra.Services
{
    public class LookupDTO
    {
        public LookupDTO()
        {
        }

        public LookupDTO(string text) : this(text, text)
        {
        }

        public LookupDTO(string key, string value)
        {
            Key = key;
            Value = value;
        }
        public LookupDTO(int key, string value)
        {
            Key = key.ToString();
            Value = value;
        }

        public string Key { get; set; }
        public string Value { get; set; }
    }
}

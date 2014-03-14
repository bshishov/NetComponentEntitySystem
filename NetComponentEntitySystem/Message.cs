namespace NetComponentEntitySystem
{
    public class Message
    {
        public readonly string Type;
        public readonly object Value;

        public Message(string type, object value)
        {
            Type = type;
            Value = value;
        }
    }
}
namespace NBi.Core
{
    public class StringComparerHelper
    {
        public string Value { get; set; }

        public static StringComparerHelper Build(string value)
        {
            return new StringComparerHelper() { Value = value };
        }
    }
}

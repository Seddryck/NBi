namespace NBi.Core.Query
{
    public class ParserResult
    {
        public bool IsSuccesful { get; set; }
        public string[] Errors { get; set; }

        protected ParserResult()
        {
            IsSuccesful = true;
        }

        public ParserResult(string[] errors)
        {
            Errors = errors;
            IsSuccesful = (Errors.Length == 0);

        }

        public static ParserResult NoParsingError()
        {
            return new ParserResult();
        }

    }
}

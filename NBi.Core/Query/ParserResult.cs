namespace NBi.Core.Query
{
    /// <summary>
    /// This class manage the result of a query parsing
    /// </summary>
    public class ParserResult
    {
        //Specify if the parsing was successful or not
        public bool IsSuccesful { get; private set; } = false;
        //Specify the list of errors met during the parsing of the query
        public string[] Errors { get; private set; } = [];

        /// <summary>
        /// Constructor without argument to specify a success of the parsing
        /// </summary>
        protected ParserResult()
        {
            IsSuccesful = true;
        }

        /// <summary>
        /// Constructor to specify errors met during the parsing of the query
        /// </summary>
        /// <param name="errors">List of errors met during pasing</param>
        public ParserResult(string[] errors)
        {
            Errors = errors;
            IsSuccesful = (Errors.Length == 0);

        }

        /// <summary>
        /// Static method to get a  for a parsing
        /// </summary>
        /// <returns>A succesful parsing result</returns>
        public static ParserResult NoParsingError()
        {
            return new ParserResult();
        }

    }
}

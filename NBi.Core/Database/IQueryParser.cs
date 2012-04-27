namespace NBi.Core.Database
{
    public interface IQueryParser
    {
        Result ValidateFormat(string sqlQuery);
    }
}

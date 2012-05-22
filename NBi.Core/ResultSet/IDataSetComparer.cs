namespace NBi.Core.ResultSet
{
    public interface IDataSetComparer
    {
        Result Validate(string sql);

        Result ValidateStructure(string sql);

        Result ValidateContent(string sql);
    }
}
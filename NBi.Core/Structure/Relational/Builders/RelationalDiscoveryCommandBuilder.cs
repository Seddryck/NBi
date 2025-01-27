using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBi.Core.Structure.Relational.Builders;

abstract class RelationalDiscoveryCommandBuilder(string captionName, string tableName) : IDiscoveryCommandBuilder
{
    protected virtual string BasicCommandText 
    {
        get => "select [{0}_name] from INFORMATION_SCHEMA.{1} where 1=1";
    }

    private string? commandText;
    private IEnumerable<IPostCommandFilter> postFilters = [];

    protected string CaptionName { get; set; } = captionName;
    protected string TableName { get; set; } = tableName;

    public void Build(IEnumerable<IFilter> filters)
    {
        commandText = BuildCommandText();

        var captionFilters = BuildCaptionFilters(filters.Where(f => f is CaptionFilter).Cast<CaptionFilter>());
        var otherFilters = BuildNonCaptionFilters(filters.Where(f => !(f is CaptionFilter)));

        var allFilters = captionFilters.Union(otherFilters).ToList();
        var commandFilters = allFilters.Where(f => f is CommandFilter).Cast<CommandFilter>();
        var valueFilters = commandFilters.Select(f => f.Value);

        foreach (var valueFilter in valueFilters)
            commandText += " and " + valueFilter;

        postFilters = allFilters.Where(f => f is IPostCommandFilter).Cast<IPostCommandFilter>();
    }

    protected abstract IEnumerable<ICommandFilter> BuildCaptionFilters(IEnumerable<CaptionFilter> filters);
    protected virtual IEnumerable<IFilter> BuildNonCaptionFilters(IEnumerable<IFilter> filters)
        => new List<ICommandFilter>();

    protected string BuildCommandText()
        => string.Format(BasicCommandText, CaptionName, TableName);

    public string GetCommandText()
        =>  commandText ?? throw new InvalidOperationException();

    public IEnumerable<IPostCommandFilter> GetPostFilters()
    {
        if (commandText is null)
            throw new InvalidOperationException();

        return postFilters;
    }

}

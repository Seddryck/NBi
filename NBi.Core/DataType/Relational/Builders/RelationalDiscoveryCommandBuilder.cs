using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBi.Core.DataType.Relational.Builders;

abstract class RelationalDiscoveryCommandBuilder : IDiscoveryCommandBuilder
{
    protected virtual string BasicCommandText
    {
        get { return "select is_nullable, data_type, character_maximum_length, numeric_precision, numeric_scale, dateTime_precision, character_set_name, collation_name, domain_name from INFORMATION_SCHEMA.columns where 1=1"; }
    }

    private string commandText = string.Empty;
    private bool isBuild = false;

    public void Build(IEnumerable<CaptionFilter> filters)
    {
        commandText = BuildCommandText();

        var allFilters = BuildFilters(filters).ToList();
        var comnandFilters = allFilters.Where(f => f is CommandFilter).Cast<CommandFilter>();
        var valueFilters = comnandFilters.Select(f => f.Value);

        foreach (var valueFilter in valueFilters)
            commandText += " and " + valueFilter;

        isBuild = true;
    }

    protected abstract IEnumerable<ICommandFilter> BuildFilters(IEnumerable<CaptionFilter> filters);

    protected string BuildCommandText()
    {
        return string.Format(BasicCommandText);
    }

    public string GetCommandText()
    {
        if (!isBuild)
            throw new InvalidOperationException();

        return commandText;
    }

}

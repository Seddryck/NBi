using DubUrl;
using DubUrl.BulkCopy;
using NBi.Core.FlatFile;
using NBi.Extensibility;

namespace NBi.Core.Decoration.DataEngineering.Commands;

internal sealed class DubUrlBulkLoadCommand(TableLoadCommandArgs args, ConnectionUrl url) : IDecorationCommand
{
    public void Execute()
    {
        var table = args.TableName.Execute() ?? string.Empty;
        var file = args.FileName.Execute() ?? string.Empty;
        if (!File.Exists(file))
            throw new ExternalDependencyNotFoundException(file);

        using var tableData = new CsvReader().ToDataTable(file);
        using var data = tableData.CreateDataReader();
        url.BulkCopy(table, data);
    }
}

using NBi.Core.Query;
using NBi.Core.DataSerialization;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NBi.Extensibility;
using NBi.Extensibility.Resolving;

namespace NBi.Core.ResultSet.Resolver;

class DataSerializationResultSetResolver : IResultSetResolver
{
    private DataSerializationResultSetResolverArgs Args { get; }

    public DataSerializationResultSetResolver(DataSerializationResultSetResolverArgs args)
        => Args = args;

    public virtual IResultSet Execute()
    {
        try
        {
            var factory = new DataSerializationProcessorFactory();
            var processor = factory.Instantiate(Args);
            var objects = processor.Execute();

            var helper = new ObjectsToRowsHelper();
            var rows = helper.Execute(objects);

            var rs = new DataTableResultSet();
            rs.Load(rows);
            return rs;
        }
        catch (NBiException ex)
        {
            throw new ResultSetUnavailableException(ex);
        }
    }
}

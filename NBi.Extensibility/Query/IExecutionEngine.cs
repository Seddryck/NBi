using System;
using System.Collections.Generic;
using System.Data;

namespace NBi.Extensibility.Query
{
    /// <summary>
    /// Interface defining methods implemented by engines able to execute queries and retrieve the result
    /// </summary>
    public interface IExecutionEngine
    {
        DataSet Execute();
        object? ExecuteScalar();
        IEnumerable<T> ExecuteList<T>();
    }
}

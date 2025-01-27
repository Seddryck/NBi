using System.Collections.Generic;
using System.Data;

namespace NBi.Core.Query.Format;

/// <summary>
/// Interface defining methods implemented by engines able to execute queries and retrieve the result
/// </summary>
public interface IFormatEngine
{
    IEnumerable<string> ExecuteFormat();
}

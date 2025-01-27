using System.Collections.Generic;
using System.Data;

namespace NBi.Core.Query.Validation;

/// <summary>
/// Interface defining methods implemented by engines able to execute queries and retrieve the result
/// </summary>
public interface IValidationEngine
{
    ParserResult Parse();
}

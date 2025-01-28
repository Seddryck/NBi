using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBi.Extensibility.Decoration;

public class CustomConditionResult
{
    public bool IsValid { get; }
    public string Message { get; }

    public CustomConditionResult(bool isValid, string message) => (IsValid, Message) = (isValid, message);

    public static CustomConditionResult SuccessfullCondition { get; } = new CustomConditionResult(true, string.Empty);
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NBi.Core.Evaluate;

public class ExpressionEvaluationResult
{
    
    public string Sentence { get; set; }
    public bool IsValid { get; set; }
    public object Expected { get; set; }
    public object Actual { get; set; }

    public ExpressionEvaluationResult(string sentence, bool isValid, object expected, object actual)
    {
        Sentence = sentence;
        IsValid = isValid;
        Expected = expected;
        Actual = actual;
    }
}

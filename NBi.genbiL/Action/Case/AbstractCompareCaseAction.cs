using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using NBi.GenbiL.Stateful;

namespace NBi.GenbiL.Action.Case;

public abstract class AbstractCompareCaseAction : ISingleCaseAction
{
    protected Func<string, IEnumerable<string>, bool> AssignCompare(OperatorType @operator)
        => @operator switch
        {
            OperatorType.Equal => Equal,
            OperatorType.Like => Like,
            _ => throw new ArgumentOutOfRangeException(),
        };
    private bool Like(string value, IEnumerable<string> patterns)
    {
        var result = false;
        foreach (var pattern in patterns)
            result |= Like(value, pattern);
        return result;
    }

    private bool Equal(string value, IEnumerable<string> patterns)
    {
        var result = false;
        foreach (var pattern in patterns)
            result |= value == pattern;
        return result;
    }

    protected virtual bool Like(string value, string pattern)
    {
        //Turn a SQL-like-pattern into regex, by turning '%' into '.*'
        //Doesn't handle SQL's underscore into single character wild card '.{1,1}',
        //        or the way SQL uses square brackets for escaping.
        //(Note the same concept could work for DOS-style wildcards (* and ?)
        var regex = new Regex("^" + pattern
                       .Replace(".", "\\.")
                       .Replace("%", ".*")
                       .Replace("\\.*", "\\%")
                       + "$");

        return regex.IsMatch(value);
    }

    protected virtual string GetOperatorText(OperatorType @operator)
    {
        switch (@operator)
        {
            case OperatorType.Equal:
                return "equal to";
            case OperatorType.Like:
                return "like";
            default:
                break;
        }
        throw new ArgumentException();
    }

    public  void Execute(GenerationState state) => Execute(state.CaseCollection.CurrentScope);

    public abstract void Execute(CaseSet testCases);

    public abstract string Display { get; }
}

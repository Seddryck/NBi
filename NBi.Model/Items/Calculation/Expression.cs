using NBi.Core.Evaluate;
using NBi.Core.ResultSet;
using NBi.Core.Transformation;
using NBi.Model.Variables;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBi.Model.Items.Calculation;

public class Expression: IColumnExpression
{
    public string Value
    {
        get => Script.Code;
        set => Script.Code = value;
    }

    public bool UsesInlineExpression() => Script.Language == LanguageType.NCalc;

    public Script Script { get; set; }

    public LanguageType Language
    {
        get => UsesInlineExpression() ? LanguageType.NCalc : Script.Language;
    }

    public string Name { get; set; }

    public int Column { get; set; }

    public ColumnType Type { get; set; }

    public string Tolerance { get; set; }

    public Expression()
    {
        Script = new Script() { Language = LanguageType.NCalc };
    }
}

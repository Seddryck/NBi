using System;
using System.Collections.Generic;
using NBi.Model.Items.Format;
using NBi.Model.Settings;

namespace NBi.Model.Constraints;

public class MatchPattern : AbstractConstraint, IReferenceFriendly
{
    public string Regex { get; set; }

    public NumericFormat NumericFormat { get; set; }

    public CurrencyFormat CurrencyFormat { get; set; }

    public bool NumericFormatSpecified
    {
        get
        {
            return !NumericFormat.IsEmpty;
        }
    }
    public bool CurrencyFormatSpecified
    {
        get
        {
            return !CurrencyFormat.IsEmpty;
        }
    }

    public MatchPattern()
    {
        NumericFormat = new NumericFormat(true);
        CurrencyFormat = new CurrencyFormat(true);
    }

    public void AssignReferences(IEnumerable<Reference> references)
    {
        NumericFormat.AssignReferences(references);
        CurrencyFormat.AssignReferences(references);
    }
}

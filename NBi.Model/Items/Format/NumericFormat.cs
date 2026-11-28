using System;
using System.Collections.Generic;
using System.Linq;
using NBi.Core.Format;
using NBi.Model.Settings;

namespace NBi.Model.Items.Format;

public class NumericFormat : INumericFormat
{
    protected bool isEmpty = false;

    public bool IsEmpty
    {
        get{ return isEmpty;}
    }

    public string Reference {get; set;}

    internal void AssignReferences(IEnumerable<Reference> references)
    {
        if (!string.IsNullOrEmpty(Reference))
            InitializeFromReferences(references, Reference);
    }

    protected virtual void InitializeFromReferences(IEnumerable<Reference> references, string value)
    {
        var refChoice = GetReference(references, value);

        if (refChoice.NumericFormat==null)
            throw new NullReferenceException(string.Format("A reference named '{0}' has been found, but no element 'numeric-format' has been defined", value));

        Initialize(refChoice.NumericFormat);
    }

    protected void Initialize(INumericFormat format)
    {
        DecimalDigits = format.DecimalDigits;
        DecimalSeparator = format.DecimalSeparator;
        GroupSeparator = format.GroupSeparator;
    }

    protected Reference GetReference(IEnumerable<Reference> references, string value)
    {
        if (references == null || references.Count() == 0)
            throw new InvalidOperationException("No reference has been defined for this constraint");

        var refChoice = references.FirstOrDefault(r => r.Name == value);
        if (refChoice == null)
            throw new IndexOutOfRangeException(string.Format("No reference named '{0}' has been defined.", value));
        return refChoice;
    }

    private int decimalDigits;
    public int DecimalDigits
    {
        get { return decimalDigits; }
        set { decimalDigits = value; isEmpty = false; }
    }

    private string decimalSeparator;
    public string DecimalSeparator
    {
        get { return decimalSeparator; }
        set { decimalSeparator = value; isEmpty = false; }
    }

    private string groupSeparator;
    public string GroupSeparator
    {
        get { return groupSeparator; }
        set { groupSeparator = value; isEmpty = false; }
    }

    public NumericFormat()
        : base()
    {
        DecimalSeparator = ".";
        GroupSeparator = ",";
    }

    public NumericFormat(bool isEmpty)
        : base()
    {
        this.isEmpty = isEmpty;
    }
}

using System;
using System.Collections;
using System.Diagnostics;

namespace NBi.Core.Analysis.Member;

public class Member
{
    
    public string UniqueName { get; set; }
    public string Caption { get; set; }
    public int Ordinal { get; set; }
    public int LevelNumber { get; set; }

    public Member(string uniqueName, string caption, int ordinal, int levelNumber)
    {
        UniqueName=uniqueName;
        Caption = caption;
        Ordinal = ordinal;
        LevelNumber = levelNumber;
    }

    public override string ToString()
    {
        return string.Format("< {0} > - {1}", Caption, UniqueName);
    }

    public class ComparerByCaption : IComparer
    {
        readonly IComparer internalComparer;

        public ComparerByCaption(bool caseSensitive)
        {
            internalComparer = caseSensitive ? StringComparer.CurrentCulture : StringComparer.CurrentCultureIgnoreCase;
        }
        
        // Calls CaseInsensitiveComparer.Compare with the parameters reversed.
        int IComparer.Compare(object? x, object? y)
        {
            if (x is Member xMember && y is StringComparerHelper yHelper)
                return internalComparer.Compare(yHelper.Value, xMember.Caption);

            if (y is Member yMember && x is StringComparerHelper xHelper)
                return internalComparer.Compare(yMember.Caption, xHelper.Value);

            if (x is Member xMember2 && y is Member yMember2)
                return internalComparer.Compare(yMember2.Caption, xMember2.Caption);

            throw new ArgumentException($"'{x!.GetType().Name}' and '{y!.GetType().Name}' cannot be compared with this comparer");
        }
    }
}

using System;
using System.Collections;
using System.Diagnostics;

namespace NBi.Core.Analysis.Member
{
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

        public class ComparerByCaption : IComparer
        {
            readonly IComparer internalComparer;

            public ComparerByCaption(bool caseSensitive)
            {
                internalComparer = caseSensitive ? StringComparer.CurrentCulture : StringComparer.CurrentCultureIgnoreCase;
            }
            
            // Calls CaseInsensitiveComparer.Compare with the parameters reversed.
            int IComparer.Compare(Object x, Object y)
            {
                if (x is Member && y is MemberCaption)
                    return internalComparer.Compare(((MemberCaption)y).Value, ((Member)x).Caption);

                if (y is Member && x is MemberCaption)
                    return internalComparer.Compare(((Member)y).Caption, ((MemberCaption)x).Value);

                if (x is Member && y is Member)
                    return internalComparer.Compare(((Member)y).Caption, ((Member)x).Caption);

                throw new Exception();
            }
        }
    }
}

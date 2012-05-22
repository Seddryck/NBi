using System;
using System.Collections;

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
            // Calls CaseInsensitiveComparer.Compare with the parameters reversed.
            int IComparer.Compare(Object x, Object y)
            {
                return ((new CaseInsensitiveComparer()).Compare(y, x));
            }
        }
    }
}

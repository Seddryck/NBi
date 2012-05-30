using System;
using System.Collections;

namespace NBi.Core.Analysis.Metadata
{
    public class Structure : IStructure
    {
        public readonly string Value;

        public Structure(string value)
        {
            Value = value;
        }



        public class ComparerByCaption : Comparer, IComparer
        {
            readonly IComparer internalComparer;

            public ComparerByCaption(bool caseSensitive)
            {
                internalComparer = caseSensitive ? StringComparer.CurrentCulture : StringComparer.CurrentCultureIgnoreCase;
            }
            
            // Calls CaseInsensitiveComparer.Compare with the parameters reversed.
            int IComparer.Compare(Object x, Object y)
            {
                if (x is IStructure && y is IStructure)
                    return internalComparer.Compare(((IStructure)y).Caption, ((IStructure)x).Caption);

                throw new Exception();
            }
        }

        public class ComparerByUniqueName : Comparer, IComparer
        {
            readonly IComparer internalComparer;

            public ComparerByUniqueName(bool caseSensitive)
            {
                internalComparer = caseSensitive ? StringComparer.CurrentCulture : StringComparer.CurrentCultureIgnoreCase;
            }
            
            // Calls CaseInsensitiveComparer.Compare with the parameters reversed.
            int IComparer.Compare(Object x, Object y)
            {
                if (x is IStructure && y is IStructure)
                    return internalComparer.Compare(((IStructure)y).UniqueName, ((IStructure)x).UniqueName);

                throw new Exception();
            }
        }

        public class Comparer : IComparer
        {
            readonly IComparer internalComparer;

            protected Comparer()
            {
            }

            public Comparer(bool caseSensitive)
            {
                internalComparer = new ComparerByCaption(caseSensitive);
            }
            
            // Calls CaseInsensitiveComparer.Compare with the parameters reversed.
            int IComparer.Compare(Object x, Object y)
            {
                if (x is IStructure && y is IStructure)
                    return internalComparer.Compare((IStructure)y, (IStructure)x);
                throw new Exception();
            }
        }

        public string Caption
        {
            get
            {
                return Value;
            }
            set
            {
                throw new NotImplementedException();
            }
        }

        public string UniqueName
        {
            get { throw new NotImplementedException(); }
        }
    }
}

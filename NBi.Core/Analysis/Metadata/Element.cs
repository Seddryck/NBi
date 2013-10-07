using System;
using System.Collections;

namespace NBi.Core.Analysis.Metadata
{
    public class Element
    {
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
                if (x is StringComparerHelper && y is IElement)
                    return internalComparer.Compare(((IElement)y).Caption, ((StringComparerHelper)x).Value);
                if (x is IElement && y is StringComparerHelper)
                    return internalComparer.Compare(((StringComparerHelper)y).Value, ((IElement)x).Caption); 
                if (x is IElement && y is IElement)
                    return internalComparer.Compare(((IElement)y).Caption, ((IElement)x).Caption);

                throw new Exception();
            }
        }

        public class ComparerByUniqueName : IComparer
        {
            readonly IComparer internalComparer;

            public ComparerByUniqueName(bool caseSensitive)
            {
                internalComparer = caseSensitive ? StringComparer.CurrentCulture : StringComparer.CurrentCultureIgnoreCase;
            }
            
            // Calls CaseInsensitiveComparer.Compare with the parameters reversed.
            int IComparer.Compare(Object x, Object y)
            {
                if (x is string && y is IElement)
                    return internalComparer.Compare(((IElement)y).UniqueName, ((string)x));
                if (x is IElement && y is string)
                    return internalComparer.Compare(((string)y), ((IElement)x).UniqueName);               
                if (x is IElement && y is IElement)
                    return internalComparer.Compare(((IElement)y).UniqueName, ((IElement)x).UniqueName);

                throw new Exception();
            }
        }
    }
}

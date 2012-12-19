using System;
using System.Collections;

namespace NBi.Core.Analysis.Metadata
{
    public class Field
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
                if (x is StringComparerHelper && y is IField)
                    return internalComparer.Compare(((IField)y).Caption, ((StringComparerHelper)x).Value);
                if (x is IField && y is StringComparerHelper)
                    return internalComparer.Compare(((StringComparerHelper)y).Value, ((IField)x).Caption); 
                if (x is IField && y is IField)
                    return internalComparer.Compare(((IField)y).Caption, ((IField)x).Caption);

                throw new Exception();
            }
        }      
    } 
    
}

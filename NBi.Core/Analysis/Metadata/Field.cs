using System;
using System.Collections;

namespace NBi.Core.Analysis.Metadata
{
    public class Field
    {
        public class ComparerByCaption : IComparer
        {
            readonly IComparer internalComparer;
            
            public bool CaseSensitive{private get; set;}

            public ComparerByCaption(bool caseSensitive)
            {
                internalComparer = caseSensitive ? StringComparer.CurrentCulture : StringComparer.CurrentCultureIgnoreCase;
                CaseSensitive = caseSensitive;
            }
            
            // Calls CaseInsensitiveComparer.Compare with the parameters reversed.
            int IComparer.Compare(Object x, Object y)
            {
                if (x is String && y is String)
                    return internalComparer.Compare(y, x);
                if (x is String && y is IField)
                    return internalComparer.Compare(((IField)y).Caption, x);
                if (x is IField && y is String)
                    return internalComparer.Compare(y, ((IField)x).Caption); 
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

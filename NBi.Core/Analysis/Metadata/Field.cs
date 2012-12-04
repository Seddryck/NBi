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

        public class ComparerByCaptionAndDisplayFolder : IComparer
        {
            readonly IComparer internalComparer;

            public ComparerByCaptionAndDisplayFolder(bool caseSensitive)
            {
                internalComparer = caseSensitive ? StringComparer.CurrentCulture : StringComparer.CurrentCultureIgnoreCase;
            }

            // Calls CaseInsensitiveComparer.Compare with the parameters reversed.
            int IComparer.Compare(Object x, Object y)
            {
                if (x is IFieldWithDisplayFolder && y is IFieldWithDisplayFolder)
                    return
                        Math.Max(
                            Math.Abs(internalComparer.Compare(((IFieldWithDisplayFolder)y).Caption, ((IFieldWithDisplayFolder)x).Caption)), 
                            Math.Abs(internalComparer.Compare(((IFieldWithDisplayFolder)y).DisplayFolder, ((IFieldWithDisplayFolder)x).DisplayFolder)));

                throw new Exception(string.Format("{0}{1}", x.GetType().Name, y.GetType().Name));
            }
        }
       
    } 
    public class FieldWithDisplayFolder : IFieldWithDisplayFolder
    {
            public string DisplayFolder { get; set; }
            public string Caption { get; set; }
    }
}

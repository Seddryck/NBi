using System;
using System.Linq;

namespace NBi.Core.Analysis.Metadata.Adomd
{
    internal class DisplayFolderFilter:PostCommandFilter
    {
        public DisplayFolderFilter(string caption)
        {
            Caption = caption;
        }

        public override bool Evaluate(object row)
        {
            if (row is MeasureRow)
                Evaluate((MeasureRow)row);

            throw new ArgumentException();
        }

        protected bool Evaluate(MeasureRow row)
        {
            return this.Caption.Equals(row.DisplayFolder);
        }
    }
}

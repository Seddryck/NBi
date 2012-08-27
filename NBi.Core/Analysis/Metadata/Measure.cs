
namespace NBi.Core.Analysis.Metadata
{

    public class Measure : IFieldWithDisplayFolder
    {
        public string UniqueName { get; private set; }
        public string Caption { get; set; }
        public string DisplayFolder { get; set; }

        public Measure(string uniqueName, string caption, string displayFolder)
        {
            UniqueName = uniqueName;
            Caption = caption;
            DisplayFolder = displayFolder;
        }

        public Measure Clone()
        {
            return new Measure(UniqueName, Caption, DisplayFolder);
        }

        public override string ToString()
        {
            return string.Format("{0} (\\{1})",Caption.ToString(), DisplayFolder.ToString());
        }
    }
}

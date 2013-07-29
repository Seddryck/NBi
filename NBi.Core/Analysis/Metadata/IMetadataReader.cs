using System.Collections.Generic;


namespace NBi.Core.Analysis.Metadata
{
    public interface IMetadataReader : IProgressStatusAware
    {
        IEnumerable<string> Sheets { get; }
        IEnumerable<string> Tracks { get; }
        string SheetName { get; set; }
        bool SupportSheets { get; }
        CubeMetadata Read();
        CubeMetadata Read(string track);
        void GetSheets();
        void GetTracks();
    }
}

using System.Collections.Generic;

namespace NBi.Core.Analysis.Metadata
{
    public interface IMetadataWriter : IProgressStatusAware
    {
        IEnumerable<string> Sheets { get; }
        string SheetName { get; set; }
        bool SupportSheets { get; }
        void Write(CubeMetadata metadata);
        void GetSheets();
    }
}

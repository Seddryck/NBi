using System;
using System.IO;

namespace NBi.Core.Analysis.Metadata
{
    public class MetadataFactory
    {
        public static IMetadataReader GetReader(string filename)
        {
            var extension = Path.GetExtension(filename);

            switch (extension)
            {
                case ".csv":
                    return new MetadataCsvReader(filename);
                case ".xls":
                    return new MetadataExcelOleDbReader(filename);
                default:
                    throw new NotImplementedException();
            }
            
        }

    }
}

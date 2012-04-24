using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace NBi.Core.Analysis.Metadata
{
    public class MetadataCsvReader : MetadataCsvAbstract, IMetadataReader
    {

        public MetadataCsvReader(string filename) : base(filename) { }

        public CubeMetadata Read()
        {
            CubeMetadata metadata = new CubeMetadata();

            RaiseProgressStatus("Processing CSV file");
            int i = 0;
    
	        int count = 0;
            using (StreamReader r = new StreamReader(Filename))
	        {
	            while (r.ReadLine() != null)
		            count++;	            
	        }  

            using (StreamReader sr = new StreamReader(Filename, Encoding.UTF8))
            {
                while (sr.Peek() >= 0)
                {
                    var row = sr.ReadLine();
                    if (i>0)
                    {
                        RaiseProgressStatus("Loading row {0} of {1}", i, count);
                        var meta = GetMetadataBasic(SplitLine(row));
                        LoadMetadata(meta, false, ref metadata); 
                    }
                    i++;
                    
                }
            }

            RaiseProgressStatus("CSV file processed");

            return metadata;
        }

        public CubeMetadata Read(string track)
        {
            throw new NotImplementedException();
        }

        private string[] SplitLine(string row)
        {
            var items = new List<string>(8);
            var list = new List<string>(row.Split(Definition.FieldSeparator));
            list.ForEach(item => items.Add(item.Replace(Definition.TextQualifier.ToString(), "")));
            return items.ToArray();
        }

        protected CsvMetadata GetMetadataBasic(string[] items)
        {
            var csvMetadata = new CsvMetadata();

            csvMetadata.perspectiveName = items[0];
            csvMetadata.measureGroupName = items[1];
            csvMetadata.measureCaption = items[2];
            csvMetadata.measureUniqueName = items[3];
            csvMetadata.dimensionCaption = items[4];
            csvMetadata.dimensionUniqueName = items[5];
            csvMetadata.hierarchyCaption = items[6];
            csvMetadata.hierarchyUniqueName = items[7];

            return csvMetadata;
        }

        protected struct CsvMetadata
        {
            public string perspectiveName;
            public string measureGroupName;
            public string measureCaption;
            public string measureUniqueName;
            public string dimensionCaption;
            public string dimensionUniqueName;
            public string hierarchyCaption;
            public string hierarchyUniqueName;
            public bool isChecked;
        }

        private void LoadMetadata(CsvMetadata r, bool filter, ref CubeMetadata metadata)
        {
            MeasureGroup mg = null;

            if ((!filter) || r.isChecked)
            {
                metadata.Perspectives.AddOrIgnore(r.perspectiveName);
                var perspective = metadata.Perspectives[r.perspectiveName];

                if (perspective.MeasureGroups.ContainsKey(r.measureGroupName))
                {
                    mg = perspective.MeasureGroups[r.measureGroupName];
                }
                else
                {
                    mg = new MeasureGroup(r.measureGroupName);
                    perspective.MeasureGroups.Add(mg);
                }

                if (!mg.Measures.ContainsKey(r.measureUniqueName))
                {
                    mg.Measures.Add(r.measureUniqueName, r.measureCaption);
                }

                Dimension dim = null;

                if (perspective.Dimensions.ContainsKey(r.dimensionUniqueName))
                {
                    dim = perspective.Dimensions[r.dimensionUniqueName];
                }
                else
                {
                    dim = new Dimension(r.dimensionUniqueName, r.dimensionCaption, new HierarchyCollection());
                    perspective.Dimensions.Add(dim);
                }

                if (!dim.Hierarchies.ContainsKey(r.hierarchyUniqueName))
                {
                    var hierarchy = new Hierarchy(r.hierarchyUniqueName, r.hierarchyCaption);
                    dim.Hierarchies.Add(r.hierarchyUniqueName, hierarchy);
                }

                if (!mg.LinkedDimensions.ContainsKey(r.dimensionUniqueName))
                    mg.LinkedDimensions.Add(dim);
            }
        }

    }
}

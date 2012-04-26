using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using NBi.Core.Analysis.Metadata;

namespace NBi.Core.Analysis.Query
{
    public class MdxBuilder
    {
        public event ProgressStatusHandler ProgressStatusChanged;

        public string PersistancePath { get; private set; }
        
        public MdxBuilder()
        {

        }

        public MdxBuilder(string persistancePath)
        {
            PersistancePath = persistancePath;
        }

        public string[] Build(CubeMetadata metadata)
        {
            return this.Build(metadata, "Children", "", false);
        }

        public string[] Build(CubeMetadata metadata, string hierarchyFunction)
        {
            return this.Build(metadata, hierarchyFunction, "", false);
        }

        public string[] Build(CubeMetadata metadata, string hierarchyFunction, string slicer)
        {
            return this.Build(metadata, hierarchyFunction, slicer, false);
        }

        public string[] Build(CubeMetadata metadata, bool notEmpty)
        {
            return this.Build(metadata, "Children", "", notEmpty);
        }

        public string[] Build(CubeMetadata metadata, string hierarchyFunction, string slicer, bool notEmpty)
        {
            var i = 0;
            var total = metadata.GetCountMembers();

            if (ProgressStatusChanged != null)
                ProgressStatusChanged(this, new ProgressStatusEventArgs(string.Format("Creating query set")));
            var res = new List<string>();
            foreach (var p in metadata.Perspectives)
            {
                foreach (var mg in p.Value.MeasureGroups)
                {
                    foreach (var dim in mg.Value.LinkedDimensions)
                    {
                        foreach (var hierarchy in dim.Value.Hierarchies)
                        {
                            foreach (var m in mg.Value.Measures)
                            {
                                var mdx = BuildMdx(p.Value.Name, m.Value.UniqueName, hierarchy.Value.UniqueName, hierarchyFunction, slicer, notEmpty);
                                if (!string.IsNullOrEmpty(PersistancePath))
                                {
                                    i++;
                                    var filename = BuildFilename(p.Value.Name, mg.Value.Name, m.Value.Caption, dim.Value.Caption, hierarchy.Value.Caption);
                                    Persist(mdx, filename);
                                    if (ProgressStatusChanged != null)
                                        ProgressStatusChanged(this, new ProgressStatusEventArgs(String.Format("Persisting query set {0} of {1}", i, total), i, total));
                                }
                               res.Add(mdx);
                            }

                        }
                    }
                
                }
            }
            if (ProgressStatusChanged != null)
                ProgressStatusChanged(this, new ProgressStatusEventArgs(string.Format("Query set created")));
            return res.ToArray();
        }

        public string BuildMdx(string perspective, string measure, string hierarchy, string hierarchyFunction, string slicer, bool NotEmpty)
        {
            var sb = new StringBuilder();
            sb.AppendLine("SELECT");
            sb.AppendFormat("\t{0} ON 0,\r\n", measure);
            if (NotEmpty)
                sb.AppendFormat("\tNONEMPTY({0}.{1}) ON 1\r\n", hierarchy, hierarchyFunction);
            else
                sb.AppendFormat("\t{0}.{1} ON 1\r\n", hierarchy, hierarchyFunction);
            sb.AppendLine("FROM");
            sb.AppendFormat("\t[{0}]\r\n", perspective);
            if (!string.IsNullOrEmpty(slicer))
            {
                sb.AppendLine("WHERE");
                sb.AppendFormat("\t{0}\r\n", slicer);
            }

            return sb.ToString();
        }

        public string BuildFilename(string perspective, string measureGroupName, string measureName, string dimensionName, string hierarchyName)
        {
            var filename = String.Format("{0} - {1} - {2} - {3} - {4}.mdx", perspective, measureGroupName, measureName, dimensionName, hierarchyName);
            foreach (var inv in Path.GetInvalidFileNameChars())
                filename.Replace(inv, '_');
            filename.Replace("€", "EUR");

            return Path.Combine(Path.GetFullPath(PersistancePath), filename);

        }

        public void Persist(string mdx, string filename)
        {
           using (StreamWriter outfile = new StreamWriter(filename))
           {
               outfile.Write(mdx);
           }
        }


    }
}

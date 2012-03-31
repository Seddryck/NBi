using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace NBi.QueryGenerator
{
    public class MdxBuilder
    {
        public string PersistancePath { get; private set; }
        
        public MdxBuilder()
        {

        }

        public MdxBuilder(string persistancePath)
        {
            PersistancePath = persistancePath;
        }

        public string[] Build(string perspective, MeasureGroups measureGroups)
        {
            return this.Build(perspective, measureGroups, "Children", "", false);
        }

        public string[] Build(string perspective, MeasureGroups measureGroups, string hierarchyFunction)
        {
            return this.Build(perspective, measureGroups, hierarchyFunction, "", false);
        }

        public string[] Build(string perspective, MeasureGroups measureGroups, string hierarchyFunction, string slicer)
        {
            return this.Build(perspective, measureGroups, hierarchyFunction, slicer, false);
        }

        public string[] Build(string perspective, MeasureGroups measureGroups, bool notEmpty)
        {
            return this.Build(perspective, measureGroups, "Children", "", notEmpty);
        }

        public string[] Build(string perspective, MeasureGroups measureGroups, string hierarchyFunction, string slicer, bool notEmpty)
        {
            var res = new List<string>();
            
            foreach (var mg in measureGroups)
            {
                foreach (var dim in mg.Value.LinkedDimensions)
                {
                    foreach (var hierarchy in dim.Value.Hierarchies)
                    {
                        foreach (var m in mg.Value.Measures)
                        {
                            var mdx = BuildMdx(perspective, m.Value.UniqueName, hierarchy.Value.UniqueName, hierarchyFunction, slicer, notEmpty);
                            if (!string.IsNullOrEmpty(PersistancePath))
                            {
                                var filename = BuildFilename(perspective, mg.Value.Name, m.Value.Caption, dim.Value.Caption, hierarchy.Value.Caption);
                                Persist(mdx, filename);
                            }
                           res.Add(mdx);
                        }

                    }
                }
                
            }
            
            return res.ToArray();
        }

        public string BuildMdx(string perspective, string measure, string hierarchy, string hierarchyFunction, string slicer, bool NotEmpty)
        {
            var sb = new StringBuilder();
            sb.AppendLine("SELECT");
            sb.AppendFormat("\t{0} ON 0,\r\n", measure);
            if (NotEmpty)
                sb.AppendFormat("\tNOTEMPTY({0}.{1}) ON 1\r\n", hierarchy, hierarchyFunction);
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

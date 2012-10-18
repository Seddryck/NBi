using System;
using System.Text.RegularExpressions;

namespace NBi.Core.Analysis.Metadata
{
    public class CubeMetadata
    {
        public PerspectiveCollection Perspectives { get; private set; }

        public CubeMetadata()
        {
            Perspectives = new PerspectiveCollection();
        }

        public CubeMetadata Clone()
        {
            var m = new CubeMetadata();
            foreach (var p in Perspectives)
                m.Perspectives.Add(p.Key, p.Value.Clone());
            return m;
        }

        public int GetItemsCount()
        {
            int total = 0;

            foreach (var p in this.Perspectives)
                foreach (var mg in p.Value.MeasureGroups)
                    foreach (var dim in mg.Value.LinkedDimensions)
                        total += mg.Value.Measures.Count * dim.Value.Hierarchies.Count;

            return total;
        }

        public int GetMeasuresCount()
        {
            int total = 0;

            foreach (var p in this.Perspectives)
                foreach (var mg in p.Value.MeasureGroups)
                        total += mg.Value.Measures.Count;

            return total;
        }

        public CubeMetadata FindMeasures(string pattern)
        {
            var result = new CubeMetadata();

            foreach (var p in this.Perspectives)
                foreach (var mg in p.Value.MeasureGroups)
                    foreach (var mea in mg.Value.Measures)
                    {
                        // Here we call Regex.Match.
                        var match = Regex.Match(mea.Value.Caption, pattern, RegexOptions.IgnoreCase);

                        // Here we check the Match instance.
                        if (match.Success)
                        {
                            result.Perspectives.AddOrIgnore(p.Key);
                            result.Perspectives[p.Key].MeasureGroups.AddOrIgnore(mg.Key);
                            result.Perspectives[p.Key].MeasureGroups[mg.Key].Measures.Add(mea.Value.Clone());
                        }
                    }
            
            return result;
        }
    }
}

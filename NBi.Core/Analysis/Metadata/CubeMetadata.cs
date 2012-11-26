using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using NBi.Core.Analysis.Metadata.Adomd;

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

        internal void Import(IEnumerable<PerspectiveRow> rows)
        {
            foreach (var row in rows)
            {
                Perspectives.AddOrIgnore(row.Name);
            }
        }

        internal void Import(IEnumerable<DimensionRow> rows)
        {
            foreach (var row in rows)
            {
                if (Perspectives.ContainsKey(row.PerspectiveName))
                {
                    Perspectives[row.PerspectiveName].Dimensions.AddOrIgnore(row.UniqueName, row.Caption);
                }
            }
        }

        internal void Import(IEnumerable<HierarchyRow> rows)
        {
            foreach (var row in rows)
            {
                if (Perspectives.ContainsKey(row.PerspectiveName))
                {
                    if (Perspectives[row.PerspectiveName].Dimensions.ContainsKey(row.DimensionUniqueName))
                    {
                        Perspectives[row.PerspectiveName].Dimensions[row.DimensionUniqueName].Hierarchies.AddOrIgnore(row.UniqueName, row.Caption);
                    }
                }
            }
        }

        internal void Import(IEnumerable<LevelRow> rows)
        {
            foreach (var row in rows)
            {
                if (Perspectives.ContainsKey(row.PerspectiveName))
                {
                    if (Perspectives[row.PerspectiveName].Dimensions.ContainsKey(row.DimensionUniqueName))
                    {
                        if (Perspectives[row.PerspectiveName].Dimensions[row.DimensionUniqueName].Hierarchies.ContainsKey(row.HierarchyUniqueName))
                        {
                            Perspectives[row.PerspectiveName]
                                .Dimensions[row.DimensionUniqueName]
                                .Hierarchies[row.HierarchyUniqueName]
                                .Levels.AddOrIgnore(row.UniqueName, row.Caption, row.Number);
                        }
                    }
                }
            }
        }

        internal void Import(IEnumerable<PropertyRow> rows)
        {
            foreach (var row in rows)
            {
                if (Perspectives.ContainsKey(row.PerspectiveName))
                {
                    if (Perspectives[row.PerspectiveName].Dimensions.ContainsKey(row.DimensionUniqueName))
                    {
                        if (Perspectives[row.PerspectiveName].Dimensions[row.DimensionUniqueName].Hierarchies.ContainsKey(row.HierarchyUniqueName))
                        {
                            if (Perspectives[row.PerspectiveName].Dimensions[row.DimensionUniqueName].Hierarchies[row.HierarchyUniqueName].Levels.ContainsKey(row.LevelUniqueName))
                            {
                                Perspectives[row.PerspectiveName]
                                    .Dimensions[row.DimensionUniqueName]
                                    .Hierarchies[row.HierarchyUniqueName]
                                    .Levels[row.LevelUniqueName]
                                    .Properties.AddOrIgnore(row.UniqueName, row.Caption);
                            }
                        }
                    }
                }
            }
        }

        internal void Import(IEnumerable<MeasureGroupRow> rows)
        {
            foreach (var row in rows)
            {
                if (Perspectives.ContainsKey(row.PerspectiveName))
                {
                    Perspectives[row.PerspectiveName].MeasureGroups.AddOrIgnore(row.Name);
                }
            }
        }

        internal void Import(IEnumerable<MeasureRow> rows)
        {
            foreach (var row in rows)
            {
                if (Perspectives.ContainsKey(row.PerspectiveName))
                {
                    if (Perspectives[row.PerspectiveName].MeasureGroups.ContainsKey(row.MeasureGroupName))
                    {
                        Perspectives[row.PerspectiveName].MeasureGroups[row.MeasureGroupName].Measures.Add(row.UniqueName, row.Caption, row.DisplayFolder);
                    }
                }
            }
        }

        internal void Link(IEnumerable<MeasureGroupRow> rows)
        {
            foreach (var row in rows)
            {
                if (Perspectives.ContainsKey(row.PerspectiveName))
                {
                    if (Perspectives[row.PerspectiveName].Dimensions.ContainsKey(row.LinkedDimensionUniqueName))
                    {
                        var linkedDimension = Perspectives[row.PerspectiveName].Dimensions[row.LinkedDimensionUniqueName];
                        if (Perspectives[row.PerspectiveName].MeasureGroups.ContainsKey(row.Name))
                        {
                            Perspectives[row.PerspectiveName].MeasureGroups[row.Name].LinkedDimensions.Add(linkedDimension);
                        }
                    }
                }
            }
        }
    }
}

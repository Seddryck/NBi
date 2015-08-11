using System;
using System.Collections.Generic;
using System.Linq;
using NBi.Core.Structure;

namespace NBi.NUnit.Structure
{
    internal class DescriptionStructureHelper
    {
        public string GetFilterExpression(IEnumerable<CaptionFilter> filters)
        {
            var texts = new List<string>();
            foreach (var filter in filters)
            {
                var text = string.Empty;
                switch (filter.Target)
                {
                    case Target.Perspectives:
                        text = "in perspective '{0}'";
                        break;
                    case Target.MeasureGroups:
                        text = "in measure-group '{0}'";
                        break;
                    case Target.Dimensions:
                        text = "in dimension '{0}'";
                        break;
                    case Target.Hierarchies:
                        text = "in hierarchy '{0}'";
                        break;
                    case Target.Levels:
                        text = "at level '{0}'";
                        break;
                    case Target.Tables:
                        text = "in table '{0}'";
                        break;
                    default:
                        break;
                }
                if (text.Length > 0)
                {
                    text = string.Format(text, filter.Caption);
                    texts.Add(text);
                }
            }
            texts.Reverse();
            return string.Join(", ", texts.ToArray());
        }

        public string GetTargetExpression(Target target)
        {
            var text = string.Empty;
            switch (target)
            {
                case Target.Perspectives:
                    text = "perspective";
                    break;
                case Target.MeasureGroups:
                    text = "measure-group";
                    break;
                case Target.Measures:
                    text = "measure";
                    break;
                case Target.Dimensions:
                    text = "dimension";
                    break;
                case Target.Hierarchies:
                    text = "hierarchy";
                    break;
                case Target.Levels:
                    text = "level";
                    break;
                case Target.Properties:
                    text = "property";
                    break;
                case Target.Sets:
                    text = "set";
                    break;
                case Target.Tables:
                    text = "table";
                    break;
                case Target.Columns:
                    text = "column";
                    break;
                case Target.Routines:
                    text = "routine";
                    break;
                default:
                    break;
            }

            return text;
        }

        public string GetTargetPluralExpression(Target target)
        {
            var text = string.Empty;
            switch (target)
            {
                case Target.Perspectives:
                    text = "perspectives";
                    break;
                case Target.MeasureGroups:
                    text = "measure-groups";
                    break;
                case Target.Measures:
                    text = "measures";
                    break;
                case Target.Dimensions:
                    text = "dimensions";
                    break;
                case Target.Hierarchies:
                    text = "hierarchies";
                    break;
                case Target.Levels:
                    text = "levels";
                    break;
                case Target.Properties:
                    text = "properties";
                    break;
                case Target.Sets:
                    text = "sets";
                    break;
                case Target.Tables:
                    text = "tables";
                    break;
                case Target.Columns:
                    text = "columns";
                    break;
                case Target.Routines:
                    text = "routines";
                    break;
                default:
                    break;
            }

            return text;
        }

        public string GetNextTargetExpression(Target target)
        {
            var text = string.Empty;
            switch (target)
            {
                case Target.MeasureGroups:
                    text = "measure";
                    break;
                case Target.Dimensions:
                    text = "hierarchy";
                    break;
                case Target.Hierarchies:
                    text = "level";
                    break;
                case Target.Levels:
                    text = "property";
                    break;
                case Target.Tables:
                    text = "column";
                    break;
                default:
                    break;
            }

            return text;
        }

        public string GetNextTargetPluralExpression(Target target)
        {
            var text = string.Empty;
            switch (target)
            {
                case Target.MeasureGroups:
                    text = "measures";
                    break;
                case Target.Dimensions:
                    text = "hierarchies";
                    break;
                case Target.Hierarchies:
                    text = "levels";
                    break;
                case Target.Levels:
                    text = "properties";
                    break;
                case Target.Tables:
                    text = "columns";
                    break;
                default:
                    break;
            }

            return text;
        }

        public string GetNotExpression(bool not)
        {
            if (!not)
                return "not";
            else
                return "a";
        }
    }
}

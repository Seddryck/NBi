using System;
using System.Collections.Generic;
using System.Linq;
using NBi.Core.Analysis.Request;

namespace NBi.NUnit.Structure
{
    internal class DescriptionStructureHelper
    {
        public string GetFilterExpression(IEnumerable<IFilter> filters)
        {
            var texts = new List<string>();
            foreach (var filter in filters)
            {
                var text = string.Empty;
                switch (filter.Target)
                {
                    case DiscoveryTarget.Perspectives:
                        text = "in perspective '{0}'";
                        break;
                    case DiscoveryTarget.MeasureGroups:
                        text = "in measure-group '{0}'";
                        break;
                    case DiscoveryTarget.Dimensions:
                        text = "in dimension '{0}'";
                        break;
                    case DiscoveryTarget.Hierarchies:
                        text = "in hierarchy '{0}'";
                        break;
                    case DiscoveryTarget.Levels:
                        text = "at level '{0}'";
                        break;
                    default:
                        break;
                }
                if (text.Length > 0)
                {
                    text = string.Format(text, filter.Value);
                    texts.Add(text);
                }
            }
            texts.Reverse();
            return string.Concat(string.Join(", ", texts.ToArray()), ".");
        }

        public string GetTargetExpression(DiscoveryTarget target)
        {
            var text = string.Empty;
            switch (target)
            {
                case DiscoveryTarget.Perspectives:
                    text = "perspective";
                    break;
                case DiscoveryTarget.MeasureGroups:
                    text = "measure-group";
                    break;
                case DiscoveryTarget.Measures:
                    text = "measure";
                    break;
                case DiscoveryTarget.Dimensions:
                    text = "dimension";
                    break;
                case DiscoveryTarget.Hierarchies:
                    text = "hierarchy";
                    break;
                case DiscoveryTarget.Levels:
                    text = "level";
                    break;
                default:
                    break;
            }

            return text;
        }

        public string GetNextTargetExpression(DiscoveryTarget target)
        {
            var text = string.Empty;
            switch (target)
            {
                case DiscoveryTarget.MeasureGroups:
                    text = "measure";
                    break;
                case DiscoveryTarget.Dimensions:
                    text = "hierarchy";
                    break;
                case DiscoveryTarget.Hierarchies:
                    text = "level";
                    break;
                case DiscoveryTarget.Levels:
                    text = "property";
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

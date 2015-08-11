using System;
using System.Collections.Generic;
using System.Linq;
using NBi.Core.DataType;

namespace NBi.NUnit.DataType
{
    internal class DescriptionDataTypeHelper
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
                case Target.Measures:
                    text = "measure";
                    break;
                case Target.Properties:
                    text = "property";
                    break;
                case Target.Columns:
                    text = "column";
                    break;
                default:
                    break;
            }

            return text;
        }

    }
}

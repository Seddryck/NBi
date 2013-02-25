using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NBi.Testing.Acceptance.Resources
{
    class AssemblyClass
    {
        public AssemblyClass()
        {

        }

        public string GetSelectMdx(string calendarYear)
        {
            var mdx = string.Format("SELECT [Measures].[Reseller Order Count] ON 0, [Date].[Calendar Year].[{0}] ON 1 FROM [Adventure Works]", calendarYear);
            return mdx;
        }

        public enum Measure
        {
            FirstMeasure = 0,
            OrderCount = 1
        }

        public string GetSelectMdxWithTwoParams(int year, Measure measure)
        {
            if (measure== Measure.OrderCount)
                return string.Format("SELECT [Measures].[Reseller Order Count] ON 0, [Date].[Calendar Year].[CY {0}] ON 1 FROM [Adventure Works]", year);
            else
                return "Incorrect Query";
        }
    }
}

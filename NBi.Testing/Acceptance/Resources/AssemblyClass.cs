using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NBi.Testing.Acceptance.Resources
{
    /// <summary>
    /// This class is only used for acceptance testing purpose
    /// </summary>
    class AssemblyClass
    {
        public AssemblyClass()
        {

        }

        /// <summary>
        /// Method returning a valid MDX query
        /// </summary>
        /// <param name="calendarYear">A year between 2001 and 2006 preceded by 'CY'</param>
        /// <returns>Valid MDX query</returns>
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

        /// <summary>
        /// Method returning a valid MDX query if the param Measure is equal to OrderCount else an invalid MDX statement
        /// </summary>
        /// <param name="year">a calendar year without the 'CY'</param>
        /// <param name="measure"></param>
        /// <returns></returns>
        public string GetSelectMdxWithTwoParams(int year, Measure measure)
        {
            if (measure== Measure.OrderCount)
                return string.Format("SELECT [Measures].[Reseller Order Count] ON 0, [Date].[Calendar Year].[CY {0}] ON 1 FROM [Adventure Works]", year);
            else
                return "Incorrect Query";
        }
    }
}

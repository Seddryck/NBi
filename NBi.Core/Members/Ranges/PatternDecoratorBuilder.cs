using System;
using System.Collections.Generic;
using System.Linq;

namespace NBi.Core.Members.Ranges
{
	internal class PatternDecoratorBuilder : BaseDecoratorBuilder
	{
		protected override IEnumerable<string> InternalApply(IEnumerable<string> results)
		{
			return ApplyPattern(results, ((IPatternDecorator)Range).Pattern, ((IPatternDecorator)Range).Position);
		}

		private IEnumerable<string> ApplyPattern(IEnumerable<string> results, string pattern, PositionValue position)
		{
			var list = new List<string>();

			Func<string, string, string> patternizer=null;
			switch (position)
			{
				case PositionValue.Suffix: 
					patternizer = ApplyPatternAsSuffix;
					break;
				case PositionValue.Prefix: 
					patternizer = ApplyPatternAsPrefix;
					break;
			}
			foreach (var value in results)
				list.Add(patternizer(pattern, value));

			return list;
			
		}

		public IEnumerable<string> Build(int start, int end, int step)
		{
			var list = new List<string>();
			for (int i = start; i <= end; i+=step)
				list.Add(i.ToString());

			return list;
		}

		protected string ApplyPatternAsPrefix(string pattern, string value)
		{
			return string.Format("{0}{1}", pattern, value);
		}

		protected string ApplyPatternAsSuffix(string pattern, string value)
		{
			return string.Format("{1}{0}", pattern, value);
		}
	}
}

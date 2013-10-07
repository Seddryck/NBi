using System;
using System.Linq;

namespace NBi.Core.ResultSet
{
	class Interval
	{
		public class EndPoint
		{
			public double Value {get;set;}
			public bool IsClosed {get;set;}
			public bool IsOpen
			{
				get
				{
					return !IsClosed;
				}
			}

			public EndPoint(double value) : this(value, true)
			{
			}

			public EndPoint(double value, bool isClosed)
			{
				Value = value;
				IsClosed = isClosed;
			}

		}

		public EndPoint Left { get; set; }
		public EndPoint Right { get; set; }

		public Interval (string value)
		{
            value = value.Replace(" ", "");
			var error = DefineParsingErrors(value);
			if (!(string.IsNullOrEmpty(error)))
				throw new ArgumentException(string.Format(error + " and was '{0}'", value));

			var split = value.Split(';');
			if (split[0].Substring(1, split[0].Length - 1).ToLower() == "-inf")
				Left = new EndPoint(double.NegativeInfinity, true);
			else
				Left = new EndPoint(
						Double.Parse(split[0].Substring(1, split[0].Length-1))
                        , split[0].StartsWith("[")
					);

			if (split[1].Substring(0, split[1].Length - 1).ToLower() == "+inf"
				|| split[1].Substring(0, split[1].Length - 1).ToLower() == "inf")
				Right = new EndPoint(double.PositiveInfinity, true);
			else
				Right = new EndPoint(
							Double.Parse(split[1].Substring(0, split[1].Length - 1))
                            , split[1].EndsWith("]")
						);
			
		}


		public bool Contains(double value)
		{
			if (value<Left.Value || value>Right.Value)
				return false;

			if (value==Left.Value && Left.IsOpen)
				return false;

			if (value == Right.Value && Right.IsOpen)
				return false;

			return true;
		}

		public bool Contains(decimal value)
		{
			return Contains(Convert.ToDouble(value));
		}


		public static bool IsValid(object x)
		{
			if (!(x is string))
				return false;

			return DefineParsingErrors((string)x) == string.Empty;

		}

		private static string DefineParsingErrors(string value)
		{
            value = value.Replace(" ", "");
			if (!(value.StartsWith("]") || value.StartsWith("[")))
				return ("The interval definition must start by '[' or ']'");
			if (!(value.EndsWith("]") || value.EndsWith("[")))
				return ("The interval definition must end by '[' or ']'");
			if (!(value.Contains(";")))
				return ("The interval definition must contain a delimitor ';'");

			if (value.Split(';').Count() > 2)
				return ("The interval definition must contain only one delimitor ';'");

			return string.Empty;
		}
	}
}

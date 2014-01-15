using System;
using System.Collections;
using System.Collections.Generic;
using NBi.Core.Analysis.Member;
using NBi.Core.Analysis.Request;
using NUnit.Framework.Constraints;
using NUnitCtr = NUnit.Framework.Constraints;

namespace NBi.NUnit.Member
{
	public class OrderedConstraint : AbstractMembersConstraint
	{
		private bool reversed;
		private IList<Object> specific;

		/// <summary>
		/// Construct a CollectionContainsConstraint specific for Members
		/// </summary>
		/// <param name="expected"></param>
		public OrderedConstraint()
			: base()
		{
			Comparer = new AlphabeticalComparer();
		}

		#region Modifiers
		/// <summary>
		/// Flag the constraint to ignore case and return self.
		/// </summary>
		public OrderedConstraint Descending
		{
			get
			{
				reversed = true;
				return this;
			}
		}
		
		/// <summary>
		/// Flag the constraint to use StringComparaison.
		/// </summary>
		public OrderedConstraint Alphabetical
		{
			get
			{
				Comparer = new AlphabeticalComparer();
				return this;
			}
		}

		/// <summary>
		/// Flag the constraint to use DateTimeComparaison.
		/// </summary>
		public OrderedConstraint Chronological
		{
			get
			{
				Comparer = new ChronologicalComparer();
				return this;
			}
		}

		/// <summary>
		/// Flag the constraint to use DecimalComparaison.
		/// </summary>
		public OrderedConstraint Numerical
		{
			get
			{
				Comparer = new NumericalComparer();
				return this;
			}
		}

		/// <summary>
		/// Flag the constraint to use DecimalComparaison.
		/// </summary>
		public OrderedConstraint Specific(IList<Object> definition)
		{
			specific = definition;
			Comparer = null;
			return this;
		}

		

		protected override NUnitCtr.Constraint BuildInternalConstraint()
		{
			var ctr = new CollectionOrderedConstraint();
			if (this.reversed)
				ctr = ctr.Descending;
			if (Comparer != null) //Should only happens if specific is called
				ctr = ctr.Using(Comparer);
			return ctr;
		}

		#endregion

		protected override bool DoMatch(NUnitCtr.Constraint ctr)
		{
				if (specific == null)
					return ctr.Matches(actual);
				else
					return SpecificMatches(actual);
		}

		protected bool SpecificMatches(object actual)
		{          
			int index=0;
			
			foreach (var item in (IEnumerable<Object>)actual)
			{
				var itemComparable = item is NBi.Core.Analysis.Member.Member ? ((NBi.Core.Analysis.Member.Member)item).Caption : item;

				int i = specific.IndexOf(itemComparable);

				if (i > -1) //found
				{
					if (i < index)
						return false;
					else
						index = i;
				}
			}

			return true;
		}

		/// <summary>
		/// Write the constraint description to a MessageWriter
		/// </summary>
		/// <param name="writer">The writer on which the description is displayed</param>
		public override void WriteDescriptionTo(NUnitCtr.MessageWriter writer)
		{

			writer.WritePredicate(string.Format("On perspective \"{0}\", the {1} of \"{2}\" are ordered {3}{4}"
															, Request.Perspective
															, Request.Function.ToLower()
															, Request.Path
															, Comparer == null ? "specifically" : ((IComparerWithLabel)Comparer).Label
															, reversed ? "(descending)" : string.Empty));
		}

		protected interface IComparerWithLabel : IComparer
		{
			string Label { get; }
		}

		protected class AlphabeticalComparer : IComparerWithLabel
		{
			private readonly IComparer internalComparer;
			
			public AlphabeticalComparer()
			{
				internalComparer = StringComparer.InvariantCultureIgnoreCase;
			}

			public string Label
			{
				get
				{
					return "alphabetically";
				}
			}

			int IComparer.Compare(Object x, Object y)
			{
				x = x is NBi.Core.Analysis.Member.Member ? ((NBi.Core.Analysis.Member.Member)x).Caption : x;
				y = y is NBi.Core.Analysis.Member.Member ? ((NBi.Core.Analysis.Member.Member)y).Caption : y;

				return internalComparer.Compare(x, y);
			}
		}

		protected class ChronologicalComparer : IComparerWithLabel
		{
			public ChronologicalComparer()
			{
			}

			public string Label
			{
				get
				{
					return "chronologically";
				}
			}
			
			int IComparer.Compare(Object x, Object y)
			{
				x = x is NBi.Core.Analysis.Member.Member ? ((NBi.Core.Analysis.Member.Member)x).Caption : x;
				y = y is NBi.Core.Analysis.Member.Member ? ((NBi.Core.Analysis.Member.Member)y).Caption : y;
				
				if (x is DateTime && y is DateTime)
					return DateTime.Compare((DateTime)x,(DateTime)y);
				if (x is DateTime && y is String)
				{
					DateTime newY;
					if (DateTime.TryParse((string)y, out newY))
						return DateTime.Compare((DateTime)x, newY);
					else
						return 0;
						//throw new ArgumentException(string.Format("'{0}' cannot be converted to DateTime", y));
				}
				if (x is String && y is DateTime)
				{
					DateTime newX;
					if (DateTime.TryParse((string)x, out newX))
						return DateTime.Compare(newX, (DateTime)y);
					else
						return 0;
						//throw new ArgumentException(string.Format("'{0}' cannot be converted to DateTime", x));
				}
				if (x is String && y is String)
				{
					DateTime newX, newY;
					if (DateTime.TryParse((string)x, out newX) && DateTime.TryParse((string)y, out newY))
						return DateTime.Compare(newX, newY);
					else
						return 0;
						//throw new ArgumentException(string.Format("'{0}' of type '{1}' or '{2}' of type '{3}' cannot be converted to DateTime", x, x.GetType().Name, y, y.GetType().Name));
				}
				

				throw new ArgumentException(string.Format("'{0}' or '{1}' cannot be compared chronologically", x.GetType().Name, y.GetType().Name));
			}
		}

		protected class NumericalComparer : IComparerWithLabel
		{
			public NumericalComparer()
			{
			}

			public string Label
			{
				get
				{
					return "numerically";
				}
			}

			int IComparer.Compare(Object x, Object y)
			{
				x = x is NBi.Core.Analysis.Member.Member ? ((NBi.Core.Analysis.Member.Member)x).Caption : x;
				y = y is NBi.Core.Analysis.Member.Member ? ((NBi.Core.Analysis.Member.Member)y).Caption : y;

				Decimal newX, newY;
				if (Decimal.TryParse(x.ToString(), out newX) && Decimal.TryParse(y.ToString(), out newY))
					return Decimal.Compare(newX, newY);
				else
					return 0;
					//throw new ArgumentException(string.Format("'{0}' of type '{1}' or '{2}' of type '{3}' cannot be converted to Decimal", x, x.GetType().Name, y, y.GetType().Name));
			}
		}
		
	}
}

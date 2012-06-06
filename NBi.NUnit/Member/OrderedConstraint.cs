using System;
using System.Collections;
using NBi.Core.Analysis;
using NBi.Core.Analysis.Member;
using NUnitCtr = NUnit.Framework.Constraints;

namespace NBi.NUnit.Member
{
    public class OrderedConstraint : NUnitCtr.CollectionOrderedConstraint
    {
        private bool reversed;
        protected IComparer comparer;
        protected DiscoverCommand command;
        protected IDiscoverMemberEngine memberEngine;

        /// <summary>
        /// Engine dedicated to MetadataExtractor acquisition
        /// </summary>
        protected internal IDiscoverMemberEngine MemberEngine
        {
            set
            {
                if (value == null)
                    throw new ArgumentNullException();
                memberEngine = value;
            }
        }

        protected IDiscoverMemberEngine GetEngine()
        {
            if (memberEngine == null)
                memberEngine = new CubeDimensionAdomdEngine();
            return memberEngine;
        }

        /// <summary>
        /// Construct a CollectionContainsConstraint specific for Members
        /// </summary>
        /// <param name="expected"></param>
        public OrderedConstraint()
            : base()
        {
            IComparer comp = StringComparer.InvariantCultureIgnoreCase;
            base.Using(comp);
        }

        #region Modifiers
        /// <summary>
        /// Flag the constraint to ignore case and return self.
        /// </summary>
        public new OrderedConstraint Descending
        {
            get
            {
                reversed = true;
                return (OrderedConstraint)base.Descending;
            }
        }
        
        /// <summary>
        /// Flag the constraint to use StringComparaison.
        /// </summary>
        public new OrderedConstraint Alphabetical
        {
            get
            {
                IComparer comp = StringComparer.InvariantCultureIgnoreCase;
                return (OrderedConstraint)base.Using(comp);
            }
        }

        /// <summary>
        /// Flag the constraint to use DateTimeComparaison.
        /// </summary>
        public new OrderedConstraint Chronological
        {
            get
            {
                IComparer comp = new ChronologicalComparer();
                return (OrderedConstraint)base.Using(comp);
            }
        }

        /// <summary>
        /// Flag the constraint to use DecimalComparaison.
        /// </summary>
        public new OrderedConstraint Numerical
        {
            get
            {
                IComparer comp = new NumericalComparer();
                return (OrderedConstraint)base.Using(comp);
            }
        }


        #endregion

        public override bool Matches(object actual)
        {
            if (actual is DiscoverCommand)
                return Process((DiscoverCommand)actual);
            else
                return base.Matches(actual);
        }

        protected bool Process(DiscoverCommand actual)
        {
            command = actual;
            var extr = GetEngine();
            MemberResult result = extr.Execute(command);
            return this.Matches(result);
        }

        protected class ChronologicalComparer : IComparer
        {
            public ChronologicalComparer()
            {
            }
            
            int IComparer.Compare(Object x, Object y)
            {
                if (x is DateTime && y is DateTime)
                    return DateTime.Compare((DateTime)x,(DateTime)y);
                if (x is DateTime && y is String)
                {
                    DateTime newY;
                    if (DateTime.TryParse((string)y, out newY))
                        return DateTime.Compare((DateTime)x, newY);
                    else
                        throw new ArgumentException(string.Format("'{0}' cannot be converted to DateTime", y));
                }
                if (x is String && y is DateTime)
                {
                    DateTime newX;
                    if (DateTime.TryParse((string)x, out newX))
                        return DateTime.Compare(newX, (DateTime)y);
                    else
                        throw new ArgumentException(string.Format("'{0}' cannot be converted to DateTime", x));
                }
                if (x is String && y is String)
                {
                    DateTime newX, newY;
                    if (DateTime.TryParse((string)x, out newX) && DateTime.TryParse((string)y, out newY))
                        return DateTime.Compare(newX, newY);
                    else
                        throw new ArgumentException(string.Format("'{0}' or '{1}' cannot be converted to DateTime", x, y));
                }

                throw new ArgumentException(string.Format("'{0}' or '{1}' cannot be compared chronologically", x.GetType().Name, y.GetType().Name));
            }
        }

        protected class NumericalComparer : IComparer
        {
            public NumericalComparer()
            {
            }

            int IComparer.Compare(Object x, Object y)
            {
                Decimal newX, newY;
                if (Decimal.TryParse(x.ToString(), out newX) && Decimal.TryParse(y.ToString(), out newY))
                    return Decimal.Compare(newX, newY);
                else
                    throw new ArgumentException(string.Format("'{0}' or '{1}' cannot be converted to DateTime", x, y));
            }
        }
    }
}

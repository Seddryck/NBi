using System;
using System.Collections;
using System.Collections.Generic;
using NBi.Core.Analysis;
using NBi.Core.Analysis.Member;
using NBi.Core.Analysis.Metadata;
using NUnitCtr = NUnit.Framework.Constraints;

namespace NBi.NUnit.Member
{
    public class OrderedConstraint : NUnitCtr.CollectionOrderedConstraint
    {
        private bool reversed;
        private IList<Object> specific;
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
        public OrderedConstraint Alphabetical
        {
            get
            {
                IComparer comp = new AlphabeticalComparer();
                return (OrderedConstraint)base.Using(comp);
            }
        }

        /// <summary>
        /// Flag the constraint to use DateTimeComparaison.
        /// </summary>
        public OrderedConstraint Chronological
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
        public OrderedConstraint Numerical
        {
            get
            {
                IComparer comp = new NumericalComparer();
                return (OrderedConstraint)base.Using(comp);
            }
        }

        /// <summary>
        /// Flag the constraint to use DecimalComparaison.
        /// </summary>
        public OrderedConstraint Specific(IList<Object> definition)
        {
            specific = definition;
            return this;
        }


        #endregion

        public override bool Matches(object actual)
        {
            if (actual is DiscoverCommand)
                return Process((DiscoverCommand)actual);
            else
            {
                if (specific == null)
                    return base.Matches(actual);
                else
                    return this.doMatch(actual);
            }
        }

        protected bool doMatch(object actual)
        {
            Console.Out.WriteLine("doMatch"); 
            
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

                //int i=0;
                //while (i != specific.Count && itemComparable != specific[i])
                //    i++;

                //if (i<specific.Count && i<index)
                //    return false;

                //if (i<specific.Count)
                //    index = i;
	        }

            return true;
        }

        protected bool Process(DiscoverCommand actual)
        {
            command = actual;
            var extr = GetEngine();
            MemberResult result = extr.Execute(command);
            return this.Matches(result);
        }

        protected class AlphabeticalComparer : IComparer
        {
            private readonly IComparer internalComparer;
            
            public AlphabeticalComparer()
            {
                internalComparer = StringComparer.InvariantCultureIgnoreCase;
            }

            int IComparer.Compare(Object x, Object y)
            {
                x = x is NBi.Core.Analysis.Member.Member ? ((NBi.Core.Analysis.Member.Member)x).Caption : x;
                y = y is NBi.Core.Analysis.Member.Member ? ((NBi.Core.Analysis.Member.Member)y).Caption : y;

                return internalComparer.Compare(x, y);
            }
        }

        protected class ChronologicalComparer : IComparer
        {
            public ChronologicalComparer()
            {
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

        protected class NumericalComparer : IComparer
        {
            public NumericalComparer()
            {
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

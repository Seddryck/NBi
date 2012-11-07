using System;
using System.Collections;
using System.Collections.Generic;
using NBi.Core.Analysis.Discovery;
using NBi.Core.Analysis.Member;
using NUnitCtr = NUnit.Framework.Constraints;

namespace NBi.NUnit.Member
{
    public class OrderedConstraint : NUnitCtr.CollectionOrderedConstraint
    {
        private bool reversed;
        private IList<Object> specific;
        protected IComparerWithLabel comparer;
        protected MembersDiscoveryCommand command;
        protected MembersAdomdEngine memberEngine;

        /// <summary>
        /// Engine dedicated to MetadataExtractor acquisition
        /// </summary>
        protected internal MembersAdomdEngine MemberEngine
        {
            set
            {
                if (value == null)
                    throw new ArgumentNullException();
                memberEngine = value;
            }
        }

        protected MembersAdomdEngine GetEngine()
        {
            if (memberEngine == null)
                memberEngine = new MembersAdomdEngine();
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
                comparer = new AlphabeticalComparer();
                return (OrderedConstraint)base.Using(comparer);
            }
        }

        /// <summary>
        /// Flag the constraint to use DateTimeComparaison.
        /// </summary>
        public OrderedConstraint Chronological
        {
            get
            {
                comparer = new ChronologicalComparer();
                return (OrderedConstraint)base.Using(comparer);
            }
        }

        /// <summary>
        /// Flag the constraint to use DecimalComparaison.
        /// </summary>
        public OrderedConstraint Numerical
        {
            get
            {
                comparer = new NumericalComparer();
                return (OrderedConstraint)base.Using(comparer);
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
            if (actual is MembersDiscoveryCommand)
                return Process((MembersDiscoveryCommand)actual);
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

        protected bool Process(MembersDiscoveryCommand actual)
        {
            command = actual;
            var extr = GetEngine();
            MemberResult result = extr.GetMembers(command);
            return this.Matches(result);
        }

        /// <summary>
        /// Write the constraint description to a MessageWriter
        /// </summary>
        /// <param name="writer">The writer on which the description is displayed</param>
        public override void WriteDescriptionTo(NUnitCtr.MessageWriter writer)
        {

            writer.WritePredicate(string.Format("On perspective \"{0}\", the {1} of \"{2}\" are ordered {3}"
                                                            , command.Perspective
                                                            , command.Function.ToLower()
                                                            , command.Path
                                                            , comparer == null ? "specifically" : comparer.Label));
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

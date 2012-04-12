using System;
using System.Collections.Generic;
using System.Collections;
using System.IO;

namespace NBi.Core.Util.DiffFiles.LongestCommonSequence
{
    public class TextLine : IComparable
	{
		public string Line;
		public int _hash;

		public TextLine(string str)
		{
			Line = str.Replace("\t","    ");
			_hash = str.GetHashCode();
		}
		#region IComparable Members

		public int CompareTo(object obj)
		{
			return _hash.CompareTo(((TextLine)obj)._hash);
		}

		#endregion
    }
}

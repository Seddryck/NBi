using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace NBi.Core.ResultSet.Formatter
{
	class ColumnFormatter
	{
		private readonly HeaderFormatter headerFormatter;
		private readonly CellFormatter cellFormatter;

		public ColumnFormatter(HeaderFormatter headerFormatter, CellFormatter cellFormatter)
		{
			this.headerFormatter = headerFormatter;
			this.cellFormatter = cellFormatter;
		}

		public void Load()
		{

		}


		protected IList<string> Tabulize(Column column, int totalWidth)
		{
			var horizontalBorder = new string('-', totalWidth);

			var list = new List<string>();
			list.Add(horizontalBorder);
			list.AddRange(headerFormatter.Tabulize(column.Header, totalWidth));
			list.Add(horizontalBorder);

            foreach (var value in column.Values)
                list.AddRange(cellFormatter.Tabulize(value, totalWidth));
				

			list.Add(horizontalBorder);

			return list;
		}

		public IList<string> Tabulize(Column column)
		{
			int maxLength = GetMaxLength(column);
			return this.Tabulize(column, maxLength);
		}

		private int GetMaxLength(Column column)
		{
			var values = new List<string>();
			foreach (var value in column.Values)
				values.Add(cellFormatter.GetDisplay(value));

			values.AddRange(headerFormatter.GetDisplay(column.Header));

			return values.Aggregate<string, int>(0, (max, next) => next.Length > max ? next.Length : max);
		}


		public string NewLine { get { return "|\r\n"; } }
	}
}

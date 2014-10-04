using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBi.Service
{
    public class TestCaseCollectionManager 
    {
        private Dictionary<string, TestCaseManager> dico;
        private string scope;
        private const string NO_NAME = "_noname";

        public TestCaseCollectionManager()
        {
            dico = new Dictionary<string, TestCaseManager>();
            connectionStrings = new Dictionary<string, string>();
        }

        public TestCaseManager Item(string name)
        {
            if (string.IsNullOrEmpty(name))
                name = NO_NAME;

            if (!dico.Keys.Contains(name))
                dico.Add(name, new TestCaseManager());

            if (dico.Count == 1)
                scope = name;

            return dico[name];
        }

        private readonly Dictionary<string, string> connectionStrings;
        public Dictionary<string, string> ConnectionStrings
        {
            get
            {
                return connectionStrings;
            }
        }

        public List<string> ConnectionStringNames
        {
            get
            {
                return ConnectionStrings.Keys.ToList();
            }
        }

        public TestCaseManager Scope
        {
            get
            {
                return Item(scope);
            }
        }

        public void SetFocus(string name)
        {
            if (!dico.Keys.Contains(name))
                dico.Add(name, new TestCaseManager());
            
            scope = name;
        }

        public void AddConnectionStrings(string name, string value)
        {
            if (connectionStrings.Keys.Contains(name))
                throw new ArgumentException("name");

            connectionStrings.Add(name, value);
        }

        public void RemoveConnectionStrings(string name)
        {
            if (!connectionStrings.Keys.Contains(name))
                throw new ArgumentException("name");

            connectionStrings.Remove(name);
        }

        public void EditConnectionStrings(string name, string newValue)
        {
            if (!connectionStrings.Keys.Contains(name))
                throw new ArgumentException("name");

            connectionStrings[name] = newValue;
        }

        public void Cross(string firstSet, string secondSet)
        {
            if (!dico.Keys.Contains(firstSet))
                throw new ArgumentException(String.Format("The test case set named '{0}' doesn't exist.", firstSet), "firstSet");

            if (!dico.Keys.Contains(secondSet))
                throw new ArgumentException(String.Format("The test case set named '{0}' doesn't exist.", secondSet), "secondSet");

            CrossContent(dico[firstSet].Content, dico[secondSet].Content, delegate { return true; });
        }

        public void Cross(string firstSet, string secondSet, string matchingColumn)
        {
            if (!dico.Keys.Contains(firstSet))
                throw new ArgumentException(String.Format("The test case set named '{0}' doesn't exist.", firstSet), "firstSet");

            if (!dico.Keys.Contains(secondSet))
                throw new ArgumentException(String.Format("The test case set named '{0}' doesn't exist.", secondSet), "secondSet");

            if (!dico[firstSet].Content.Columns.Contains(matchingColumn))
                throw new ArgumentException(String.Format("The test case set named '{0}' doesn't contain a column named '{1}'.", firstSet, matchingColumn));

            if (!dico[secondSet].Content.Columns.Contains(matchingColumn))
                throw new ArgumentException(String.Format("The test case set named '{0}' doesn't contain a column named '{1}'.", secondSet, matchingColumn));

            Func<DataRow, DataRow, bool> matchingRow = (a, b) => a[matchingColumn].Equals(b[matchingColumn]);
            CrossContent(dico[firstSet].Content, dico[secondSet].Content, matchingRow);
        }

        private void CrossContent(DataTable first, DataTable second, Func<DataRow, DataRow, bool> matchingRow)
        {
            var table = BuildStructure(first, second);

            foreach(DataRow firstRow in first.Rows)
            {
                foreach (DataRow secondRow in second.Rows)
                {
                    if (matchingRow(firstRow, secondRow))
                    {
                        var newRow = table.NewRow();
                        foreach (DataColumn column in firstRow.Table.Columns)
                            newRow[column.ColumnName] = firstRow[column.ColumnName];
                        foreach (DataColumn column in secondRow.Table.Columns)
                            newRow[column.ColumnName] = secondRow[column.ColumnName];
                        table.Rows.Add(newRow);
                    }
                }
            }

            var dataReader = table.CreateDataReader();
            Scope.Content.Clear();
            Scope.Content.Load(dataReader, LoadOption.PreserveChanges);
            Scope.Content.AcceptChanges();
            Scope.Variables.Clear();
            foreach (DataColumn column in Scope.Content.Columns)
                Scope.Variables.Add(column.ColumnName);
        }



        private DataTable BuildStructure(DataTable firstSet, DataTable secondSet)
        {
            var table = new DataTable();
            foreach (DataColumn column in firstSet.Columns)
                table.Columns.Add(column.ColumnName);
            foreach (DataColumn column in secondSet.Columns)
                if (!table.Columns.Contains(column.ColumnName))
                    table.Columns.Add(column.ColumnName);

            return table;
        }



    }
}

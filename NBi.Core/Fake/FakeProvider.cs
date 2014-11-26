using Microsoft.SqlServer.Management.Smo;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBi.Core.Fake
{
    public class FakeProvider
    {
        private string serverName;
        private string databaseName;

        public Database Database { get; private set; }
        
        public FakeProvider(string serverName, string databaseName)
        {
            this.serverName = serverName;
            this.databaseName = databaseName;
        }

        public FakeProvider(string connStr)
        {
            var builder = new DbConnectionStringBuilder();
            builder.ConnectionString = connStr.Replace("{", "\"").Replace("}", "\"");
            serverName =  (string)builder["Data Source"];
            databaseName = (string)builder["Initial Catalog"];
        }

        public IFakeInstance CreateInstance(string schema, string name)
        {
            if(Database==null)
                Database = Connect(serverName, databaseName);

            var dbObject = Find(Database, schema, name);

            var fakeInstance = BuildFakeInstance(dbObject);
            fakeInstance.Initialize();
            return fakeInstance;
        }

        protected Database Connect(string serverName, string databaseName)
        {
            var server = new Server(serverName);
            if (server.Databases.Contains(databaseName))
                return server.Databases[databaseName];
            else
                throw new ArgumentOutOfRangeException("databaseName", string.Format("No database named '{0}' found on the server '{1}'", databaseName,serverName);
        }

        protected ScriptSchemaObjectBase Find(Database database, string schema, string name)
        {
            var types = DatabaseObjectTypes.StoredProcedure | DatabaseObjectTypes.UserDefinedFunction | DatabaseObjectTypes.View;
            var table = database.EnumObjects(types);

            foreach (System.Data.DataRow row in table.Rows)
                if ((row[1] == schema || schema == null) && row[2] == name)
                    return BuildScriptSchemaObject(database, (string)row[0], (string)row[1], (string)row[2]);

            throw new ArgumentException(string.Format("No object named '{0}' found in schema '{1}' for database '{2}'", name, schema, database.Name));
        }

        protected IFakeInstance BuildFakeInstance(ScriptSchemaObjectBase dbObject)
        {
            if (dbObject is StoredProcedure)
                return new StoredProcedureFake(dbObject as StoredProcedure);
            if (dbObject is UserDefinedFunction)
                return new UserDefinedFunctionFake(dbObject as UserDefinedFunction);
            if (dbObject is View)
                return new ViewFake(dbObject as View);

            throw new ArgumentOutOfRangeException();
        }

        private ScriptSchemaObjectBase BuildScriptSchemaObject(Database database, string databaseObjectTypeName, string schema, string name)
        {
            DatabaseObjectTypes databaseObjectType = (DatabaseObjectTypes)Enum.Parse(typeof(DatabaseObjectTypes), databaseObjectTypeName, true);
            switch (databaseObjectType)
            {
                case DatabaseObjectTypes.StoredProcedure:
                        return database.StoredProcedures[name,schema];
                case DatabaseObjectTypes.UserDefinedFunction:
                    return database.UserDefinedFunctions[name, schema];
                case DatabaseObjectTypes.View:
                    return database.Views[name, schema];
                default:
                    throw new ArgumentOutOfRangeException("databaseObjectTypeName", string.Format("The value {0} is not supported", databaseObjectTypeName));
            }
        }

        
    }
}

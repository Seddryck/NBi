#if ! SqlServer2008R2
using Microsoft.SqlServer.Management.IntegrationServices;
using System.Collections.ObjectModel;
using System.Data;
using System.Data.SqlClient;
using NBi.Core.Etl;

/// <summary>
/// From Ben E: http://muxtonmumbles.blogspot.be/2013/09/specifying-timeout-for-ssis-2012.html
/// </summary>
public static class Extensions
{
    #region Private constants

    private const string CREATE_EXECUTION = @"[catalog].[create_execution]";
    private const string SET_EXECUTION_PARAMETER_VALUE = @"[catalog].[set_execution_parameter_value]";
    private const string SET_EXECUTION_PROPERTY_OVERRIDE_VALUE = @"[catalog].[set_execution_property_override_value]";
    private const string START_EXECUTION = @"[catalog].[start_execution]";

    private const string EXECUTION_ID = @"@execution_id";
    private const string OBJECT_TYPE = @"@object_type";
    private const string PARAMETER_NAME = @"@parameter_name";
    private const string PARAMETER_VALUE = @"@parameter_value";
    private const string PACKAGE_NAME = @"@package_name";
    private const string FOLDER_NAME = @"@folder_name";
    private const string PROJECT_NAME = @"@project_name";
    private const string USE32BITRUNTIME = @"@use32bitruntime";
    private const string REFERENCE_ID = @"@reference_id";
    private const string PROPERTY_PATH = @"@property_path";
    private const string PROPERTY_VALUE = @"@property_value";
    private const string SENSITIVE = @"@sensitive";

    #endregion

    #region Public extension methods

    public static long Execute(this PackageInfo packageInfo,
        bool use32RuntimeOn64, EnvironmentReference reference,
        Collection<PackageInfo.ExecutionValueParameterSet> setValueParameters,
        int commandTimeout, string connectionString)
    {
        return packageInfo.Execute(use32RuntimeOn64, reference, setValueParameters, null, commandTimeout, connectionString);
    }

    public static long Execute(this PackageInfo packageInfo,
        bool use32RuntimeOn64, EnvironmentReference reference,
        Collection<PackageInfo.ExecutionValueParameterSet> setValueParameters,
        Collection<PackageInfo.PropertyOverrideParameterSet> propertyOverrideParameters,
        int commandTimeout, string connectionString)
    {
        long executionId = 0;

        //TODO investigate further why it's not working anymore: 
        //  STACK: Method not found: 'Microsoft.SqlServer.Management.Sdk.Sfc.SqlStoreConnection Microsoft.SqlServer.Management.IntegrationServices.IntegrationServices.get_Connection()'
        //connectionString = packageInfo.Parent.Parent.Parent.Parent.Connection.ServerConnection.ConnectionString;

        using (var connection = new SqlConnection(connectionString))
        {
            connection.Open();
            executionId = CreateExecution(packageInfo, use32RuntimeOn64,
                reference, commandTimeout, connection);

            SetExecutionParameterValues(packageInfo, setValueParameters,
                commandTimeout, connection, executionId);

            SetPropertyOverrideParameters(packageInfo, propertyOverrideParameters,
                commandTimeout, connection, executionId);

            StartExecution(commandTimeout, executionId, connection, packageInfo);
        }

        return executionId;
    }

    #endregion

    #region Private methods

    private static void StartExecution(int commandTimeout,
        long executionId, SqlConnection connection, PackageInfo packageInfo)
    {
        string storedProcName = string.Format("{0}.{1}",
        packageInfo.Parent.Parent.Parent.Name, START_EXECUTION);

        using (var command = new SqlCommand(storedProcName, connection))
        {
            command.CommandType = CommandType.StoredProcedure;
            command.CommandTimeout = commandTimeout;

            command.Parameters.Add(EXECUTION_ID, SqlDbType.Int);
            command.Parameters[EXECUTION_ID].Value = executionId;
            command.ExecuteNonQuery();
        }
    }

    private static long CreateExecution(PackageInfo packageInfo,
        bool use32RuntimeOn64, EnvironmentReference reference,
        int commandTimeout, SqlConnection connection)
    {
        long executionId = 0;
        string storedProcName = string.Format("{0}.{1}",
            packageInfo.Parent.Parent.Parent.Name, CREATE_EXECUTION);

        using (var command = new SqlCommand(storedProcName, connection))
        {
            command.CommandType = CommandType.StoredProcedure;
            command.CommandTimeout = commandTimeout;

            command.Parameters.Add(FOLDER_NAME, SqlDbType.NVarChar, 128);
            command.Parameters.Add(PROJECT_NAME, SqlDbType.NVarChar, 128);
            command.Parameters.Add(PACKAGE_NAME, SqlDbType.NVarChar, 260);
            command.Parameters.Add(REFERENCE_ID, SqlDbType.BigInt);
            command.Parameters.Add(USE32BITRUNTIME, SqlDbType.Bit);
            command.Parameters.Add(EXECUTION_ID, SqlDbType.Int);
            command.Parameters[EXECUTION_ID].Direction = ParameterDirection.Output;

            command.Parameters[FOLDER_NAME].Value = packageInfo.Parent.Parent.Name;
            command.Parameters[PROJECT_NAME].Value = packageInfo.Parent.Name;
            command.Parameters[PACKAGE_NAME].Value = packageInfo.Name;

            if (reference != null)
            {
                command.Parameters[REFERENCE_ID].Value = reference.ReferenceId;
            }

            command.Parameters[USE32BITRUNTIME].Value = use32RuntimeOn64;

            command.ExecuteNonQuery();
            executionId = long.Parse(command.Parameters[EXECUTION_ID].Value.ToString());
        }

        return executionId;
    }

    private static void SetExecutionParameterValues(PackageInfo packageInfo,
        Collection<PackageInfo.ExecutionValueParameterSet> setValueParameters,
        int commandTimeout, SqlConnection connection, long executionId)
    {
        if (setValueParameters != null && setValueParameters.Count > 0)
        {
            string storedProcName = string.Format("{0}.{1}",
                packageInfo.Parent.Parent.Parent.Name, SET_EXECUTION_PARAMETER_VALUE);

            using (var command = new SqlCommand(storedProcName, connection))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.CommandTimeout = commandTimeout;

                command.Parameters.Add(EXECUTION_ID, SqlDbType.Int);
                command.Parameters.Add(OBJECT_TYPE, SqlDbType.SmallInt);
                command.Parameters.Add(PARAMETER_NAME, SqlDbType.NVarChar, 128);
                command.Parameters.Add(PARAMETER_VALUE, SqlDbType.Variant);
                command.Parameters[EXECUTION_ID].Value = executionId;

                foreach (var setValueParameter in setValueParameters)
                {
                    command.Parameters[OBJECT_TYPE].Value = setValueParameter.ObjectType;
                    command.Parameters[PARAMETER_NAME].Value = setValueParameter.ParameterName;
                    command.Parameters[PARAMETER_VALUE].Value = setValueParameter.ParameterValue;
                    command.ExecuteNonQuery();
                }
            }
        }
    }

    private static void SetPropertyOverrideParameters(PackageInfo packageInfo,
        Collection<PackageInfo.PropertyOverrideParameterSet> propertyOverrideParameters,
        int commandTimeout, SqlConnection connection, long executionId)
    {
        if (propertyOverrideParameters != null && propertyOverrideParameters.Count > 0)
        {
            string storedProcName = string.Format("{0}.{1}",
                packageInfo.Parent.Parent.Parent.Name, SET_EXECUTION_PROPERTY_OVERRIDE_VALUE);

            using (var command = new SqlCommand(storedProcName, connection))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.CommandTimeout = commandTimeout;

                command.Parameters.Add(EXECUTION_ID, SqlDbType.Int);
                command.Parameters.Add(PROPERTY_PATH, SqlDbType.NVarChar, 4000);
                command.Parameters.Add(PROPERTY_VALUE, SqlDbType.NVarChar, -1);
                command.Parameters.Add(SENSITIVE, SqlDbType.Bit);
                command.Parameters[EXECUTION_ID].Value = executionId;

                foreach (var propertyOverrideParameter in propertyOverrideParameters)
                {
                    command.Parameters[PROPERTY_PATH].Value = propertyOverrideParameter.PropertyPath;
                    command.Parameters[PROPERTY_VALUE].Value = propertyOverrideParameter.PropertyValue;
                    command.Parameters[SENSITIVE].Value = propertyOverrideParameter.Sensitive;
                    command.ExecuteNonQuery();
                }
            }
        }
    }

    #endregion
}
#endif
[assembly: System.Runtime.CompilerServices.InternalsVisibleTo("CW.Extensions.Dapper.Test")]

namespace Dapper
{
    using System.Collections.Generic;
    using System.Data;
    using System.Diagnostics;

    /// <summary>
    /// DapperExt
    /// </summary>
    public static partial class DapperExt
    {
        public static int ExecInsert(this IDbConnection connection, dynamic data, string table, IDbTransaction transaction = null, int? commandTimeout = null)
        {
            var obj = data as object;
            var sql = BuildInsertSql(obj, table);

            if (Debugger.IsAttached)
                Debug.Write(sql);

            return connection.Execute(sql, obj, transaction, commandTimeout);
        }

        public static int ExecUpdate(this IDbConnection connection, dynamic data, dynamic condition, string table, IDbTransaction transaction = null, int? commandTimeout = null)
        {
            var updateFields = BuildUpdateFields(data);
            (string Fields, IDictionary<string, object> Params) where = BuildConditionFields(condition);
            var sql = $"UPDATE {table} SET {updateFields} {where.Fields}";

            if (Debugger.IsAttached)
                Debug.Write(sql);

            var parameters = new DynamicParameters(data);
            parameters.AddDynamicParams(where.Params);
            return connection.Execute(sql, parameters, transaction, commandTimeout);
        }

        public static int ExecUpdateNotNull(this IDbConnection connection, dynamic data, dynamic condition, string table, IDbTransaction transaction = null, int? commandTimeout = null)
        {
            var updateFields = BuildUpdateFieldsForNotNull(data);
            (string Fields, IDictionary<string, object> Params) where = BuildConditionFields(condition);
            var sql = $"UPDATE {table} SET {updateFields} {where.Fields}";

            if (Debugger.IsAttached)
                Debug.Write(sql);

            var parameters = new DynamicParameters(data);
            parameters.AddDynamicParams(where.Params);
            return connection.Execute(sql, parameters, transaction, commandTimeout);
        }

        public static long ExecGetCount(this IDbConnection connection, dynamic condition, string table, IDbTransaction transaction = null, int? commandTimeout = null)
        {
            (string Fields, IDictionary<string, object> Params) where = BuildConditionFields(condition);
            var sql = $"SELECT COUNT(*) FROM {table} {where.Fields}";

            if (Debugger.IsAttached)
                Debug.Write(sql);

            var parameters = new DynamicParameters(where.Params);
            return connection.ExecuteScalar<long>(sql, parameters, transaction, commandTimeout);
        }

        public static long ExecGetCount(this IDbConnection connection, string where, dynamic whereObj, string table, IDbTransaction transaction = null, int? commandTimeout = null)
        {
            var sql = $"SELECT COUNT(*) FROM {table} WHERE {where}";

            if (Debugger.IsAttached)
                Debug.Write(sql);

            var parameters = new DynamicParameters(whereObj);
            return connection.ExecuteScalar<long>(sql, parameters, transaction, commandTimeout);
        }
    }
}

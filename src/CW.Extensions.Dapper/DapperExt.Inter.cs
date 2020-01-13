[assembly: System.Runtime.CompilerServices.InternalsVisibleTo("CW.Extensions.Dapper.Test")]

namespace Dapper
{
    using System.Collections.Generic;

    /// <summary>
    /// DapperExt
    /// </summary>
    public static partial class DapperExt
    {
        internal static string Insert(dynamic data, string table)
        {
            var obj = data as object;
            var sql = BuildInsertSql(obj, table);
            return sql;
        }

        internal static string Update(dynamic data, dynamic condition, string table)
        {
            var updateFields = BuildUpdateFields(data);
            (string Fields, IDictionary<string, object> Params) where = BuildConditionFields(condition);
            var sql = $"UPDATE {table} SET {updateFields} {where.Fields}";
            return sql;
        }

        internal static string UpdateNotNull(dynamic data, dynamic condition, string table)
        {
            var updateFields = BuildUpdateFieldsForNotNull(data);
            (string Fields, IDictionary<string, object> Params) where = BuildConditionFields(condition);
            var sql = $"UPDATE {table} SET {updateFields} {where.Fields}";
            return sql;
        }

        internal static string GetCount(dynamic condition, string table)
        {
            (string Fields, IDictionary<string, object> Params) where = BuildConditionFields(condition);
            var sql = $"SELECT COUNT(*) FROM {table} {where.Fields}";

            return sql;
        }

        internal static string GetCount(string where, dynamic whereObj, string table)
        {
            var sql = $"SELECT COUNT(*) FROM {table} WHERE {where}";
            return sql;
        }
    }
}

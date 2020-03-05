namespace Dapper
{
    using System;
    using System.Collections.Concurrent;
    using System.Collections.Generic;
    using System.Dynamic;
    using System.Linq;
    using System.Reflection;
    using System.Text;

    /// <summary>
    /// DapperExt
    /// </summary>
    public static partial class DapperExt
    {
        private static readonly ConcurrentDictionary<Type, List<PropertyInfo>> _paramCache = new ConcurrentDictionary<Type, List<PropertyInfo>>();
        private static readonly ConcurrentDictionary<string, string> _colNameCache = new ConcurrentDictionary<string, string>();

        private static string BuildInsertSql(object obj, string table)
        {
            var dbColumnProps = GetDbColumnProperties(obj);
            var properties = GetProperties(obj);
            var columns = string.Join(", ", dbColumnProps);
            var values = string.Join(", ", properties.Select(p => $"@{p}"));
            var sql = $"INSERT INTO {table} ({columns}) VALUES ({values})";
            return sql;
        }

        private static (string Fields, IDictionary<string, object> Params) BuildConditionFields(dynamic condition)
        {
            var conditionObj = condition as object;
            var wherePropertyInfos = GetPropertyInfos(conditionObj);
            var whereProperties = wherePropertyInfos.Select(p => p.Name);
            var whereFields = string.Empty;

            if (whereProperties.Any()) whereFields = $"WHERE {string.Join(" AND ", whereProperties.Select(p => $"{p} = @w_{p}"))}";

            var expandoObject = new ExpandoObject() as IDictionary<string, object>;
            wherePropertyInfos.ForEach(p => expandoObject.Add($"w_{p.Name}", p.GetValue(conditionObj, null)));

            return (whereFields, expandoObject);
        }

        private static string BuildUpdateFieldsForNotNull(dynamic data)
        {
            var obj = data as object;

            var updatePropertyInfos = GetPropertyInfos(obj);

            var sb = new StringBuilder(1024);

            for (var i = 0; i < updatePropertyInfos.Count; i++)
            {
                var p = updatePropertyInfos[i];

                var columnName = GetDbColumnName(p);

                if (columnName.Equals("id", StringComparison.OrdinalIgnoreCase)) continue;

                if (p.PropertyType == typeof(string))
                {
                    if (!string.IsNullOrWhiteSpace(Convert.ToString(p.GetValue(obj))))
                    {
                        sb.AppendFormat("{0} = @{1}, ", columnName, p.Name);
                    }
                    else
                    {
                        continue;
                    }
                }
                else if (p.PropertyType.IsGenericType
                    && p.PropertyType.GetGenericTypeDefinition().Equals(typeof(Nullable<>)))
                {
                    if (p.GetValue(obj) != null)
                    {
                        sb.AppendFormat("{0} = @{1}, ", columnName, p.Name);
                    }
                    else
                    {
                        continue;
                    }
                }
                else
                {
                    sb.AppendFormat("{0} = @{1}, ", columnName, p.Name);
                }
            }

            return sb.ToString().Substring(0, sb.Length - 2);
        }

        private static string BuildUpdateFields(dynamic data)
        {
            var obj = data as object;

            var updatePropertyInfos = GetPropertyInfos(obj);

            var sb = new StringBuilder(1024);

            for (var i = 0; i < updatePropertyInfos.Count; i++)
            {
                var p = updatePropertyInfos[i];

                var columnName = GetDbColumnName(p);

                if (columnName.Equals("id", StringComparison.OrdinalIgnoreCase)) continue;

                sb.AppendFormat("{0} = @{1}", columnName, p.Name);
                if (i < updatePropertyInfos.Count - 1)
                    sb.AppendFormat(", ");
            }

            return sb.ToString();
        }

        private static List<string> GetProperties(object obj)
        {
            if (obj == null) return new List<string>();

            if (obj is DynamicParameters) return (obj as DynamicParameters).ParameterNames.ToList();

            return GetPropertyInfos(obj).Select(x => x.Name).ToList();
        }

        private static List<string> GetDbColumnProperties(object obj)
        {
            if (obj == null) return new List<string>();

            if (obj is DynamicParameters) return (obj as DynamicParameters).ParameterNames.ToList();

            return GetPropertyInfos(obj).Select(x => GetDbColumnName(x)).ToList();
        }

        private static string GetDbColumnName(PropertyInfo p)
        {
            string key = $"{p.DeclaringType}.{p.Name}";
            if (_colNameCache.TryGetValue(key, out var columnName)) return columnName;

            var name = p.Name;
            dynamic attr = p.GetCustomAttributes(true).FirstOrDefault(y => y.GetType().Name == typeof(DbColumnAttribute).Name);
            if (attr != null) name = attr.Name;

            _colNameCache.AddOrUpdate(key, name, (t, v) => name);

            return name;
        }

        private static List<PropertyInfo> GetPropertyInfos(object obj)
        {
            if (obj == null) return new List<PropertyInfo>();

            Type type = obj.GetType();

            if (_paramCache.TryGetValue(type, out List<PropertyInfo> properties)) return properties.ToList();

            properties = type.GetProperties(BindingFlags.GetProperty | BindingFlags.Instance | BindingFlags.Public).ToList();
            _paramCache[type] = properties;
            return properties;
        }
    }
}

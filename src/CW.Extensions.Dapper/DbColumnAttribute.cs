namespace Dapper
{
    using System;

    [AttributeUsage(AttributeTargets.Property)]
    public class DbColumnAttribute : Attribute
    {
        public DbColumnAttribute(string columnName)
        {
            Name = columnName;
        }

        public string Name { get; private set; }
    }
}

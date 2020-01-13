namespace CW.Extensions.Dapper.Test
{
    using global::Dapper;

    public class Demo2
    {
        public int Id { get; set; }

        [DbColumn("myName")]
        public string Name { get; set; }

        public string Ext1 { get; set; }

        public int? Ext2 { get; set; }
    }
}

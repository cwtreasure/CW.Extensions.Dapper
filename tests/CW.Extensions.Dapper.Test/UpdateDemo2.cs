namespace CW.Extensions.Dapper.Test
{
    using global::Dapper;

    public class UpdateDemo2
    {
        [DbColumn("myName")]
        public string Name { get; set; }

        public string Ext1 { get; set; }

        public string Ext2 { get; set; }
    }
}

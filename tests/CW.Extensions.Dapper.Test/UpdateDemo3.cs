namespace CW.Extensions.Dapper.Test
{
    using global::Dapper;

    public class UpdateDemo3
    {
        [DbColumn("id")]
        public int Id { get; set; }

        [DbColumn("last_name")]
        public string Name { get; set; }
    }
}

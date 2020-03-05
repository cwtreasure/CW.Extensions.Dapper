namespace CW.Extensions.Dapper.Test
{
    using global::Dapper;

    public class UpdateDemo4
    {
        [DbColumn("a_id")]
        public int Id { get; set; }

        [DbColumn("last_name")]
        public string Name { get; set; }
    }
}

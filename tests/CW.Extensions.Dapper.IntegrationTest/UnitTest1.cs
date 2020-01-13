namespace CW.Extensions.Dapper.IntegrationTest
{
    using global::Dapper;
    using Npgsql;
    using System.Threading.Tasks;
    using Xunit;

    public class UnitTest1
    {
        private const string _connString = "Host=127.0.0.1;Username=postgres;Password=123456;Database=demo";

        [Fact]
        public void Add_With_DbColumn()
        {
            using var conn = new NpgsqlConnection(_connString);
            conn.Open();

            var res = conn.Add(new SampleInsert { Name = "sssss", Gender = 1, CreateTime = System.DateTimeOffset.Now.ToUnixTimeSeconds() }, "t1");

            Assert.Equal(1, res);
        }

        [Fact]
        public void ModifyNotNull_With_DbColumn()
        {
            using var conn = new NpgsqlConnection(_connString);
            conn.Open();

            var res = conn.ModifyNotNull(new SampleUpdate { Gender = 2, }, new { id = 18 }, "t1");

            Assert.Equal(1, res);
        }

        [Fact]
        public void Modify_With_DbColumn()
        {
            using var conn = new NpgsqlConnection(_connString);
            conn.Open();

            var res = conn.Modify(new SampleUpdate { Gender = 2, CreateTime = 123, Name = "name123" }, new { id = 18 }, "t1");

            Assert.Equal(1, res);
        }

        [Fact]
        public void GetCount()
        {
            using var conn = new NpgsqlConnection(_connString);
            conn.Open();

            var count = conn.GetCount(new { id = 18 }, "t1");

            Assert.Equal(1, count);
        }

        [Fact]
        public async Task AddAsync_With_DbColumn()
        {
            using var conn = new NpgsqlConnection(_connString);
            await conn.OpenAsync();

            var res = await conn.AddAsync(new SampleInsert { Name = "sssss", Gender = 1, CreateTime = System.DateTimeOffset.Now.ToUnixTimeSeconds() }, "t1");

            Assert.Equal(1, res);
        }

        [Fact]
        public async Task ModifyNotNullAsync_With_DbColumn()
        {
            using var conn = new NpgsqlConnection(_connString);
            await conn.OpenAsync();

            var res = await conn.ModifyNotNullAsync(new SampleUpdate { Gender = 2, }, new { id = 18 }, "t1");

            Assert.Equal(1, res);
        }

        [Fact]
        public async Task ModifyAsync_With_DbColumn()
        {
            using var conn = new NpgsqlConnection(_connString);
            await conn.OpenAsync();

            var res = await conn.ModifyAsync(new SampleUpdate { Gender = 2, CreateTime = 123, Name = "name123" }, new { id = 18 }, "t1");

            Assert.Equal(1, res);
        }

        [Fact]
        public async Task GetCountAsync()
        {
            using var conn = new NpgsqlConnection(_connString);
            await conn.OpenAsync();

            var count = await conn.GetCountAsync(new { id = 18 }, "t1");

            Assert.Equal(1, count);
        }

        public class SampleInsert
        {
            [DbColumn("name")]
            public string Name { get; set; }

            [DbColumn("gender")]
            public int Gender { get; set; }

            [DbColumn("create_time")]
            public long CreateTime { get; set; }
        }

        public class SampleUpdate
        {
            [DbColumn("name")]
            public string Name { get; set; }

            [DbColumn("gender")]
            public int Gender { get; set; }

            [DbColumn("create_time")]
            public long? CreateTime { get; set; }
        }
    }
}

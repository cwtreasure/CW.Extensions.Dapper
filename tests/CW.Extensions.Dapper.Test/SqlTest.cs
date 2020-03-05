namespace CW.Extensions.Dapper.Test
{
    using global::Dapper;
    using Xunit;

    public class SqlTest
    {
        [Fact]
        public void InsertTest()
        {
            var data = new Demo { Id = 1, Name = "cat" };
            var sql = DapperExt.Insert(data, "myt");

            var exp = "INSERT INTO myt (Id, Name, Ext1, Ext2, Ext3) VALUES (@Id, @Name, @Ext1, @Ext2, @Ext3)";

            Assert.Equal(exp, sql);
        }

        [Fact]
        public void InsertTest2()
        {
            var data = new Demo2 { Id = 1, Name = "cat" };
            var sql = DapperExt.Insert(data, "myt1");

            var exp = "INSERT INTO myt1 (Id, myName, Ext1, Ext2) VALUES (@Id, @Name, @Ext1, @Ext2)";

            Assert.Equal(exp, sql);
        }

        [Fact]
        public void UpdateNotNullTest()
        {
            var data = new UpdateDemo { Name = "cat", Ext1 = "ext1" };
            var con = new { id = 1 };
            var sql = DapperExt.UpdateNotNull(data, con, "myt");

            var exp = "UPDATE myt SET Name = @Name, Ext1 = @Ext1 WHERE id = @w_id";

            Assert.Equal(exp, sql);
        }

        [Fact]
        public void UpdateNotNullTest2()
        {
            var data = new UpdateDemo2 { Name = "cat", Ext1 = "ext1" };
            var con = new { id = 1 };
            var sql = DapperExt.UpdateNotNull(data, con, "myt");

            var exp = "UPDATE myt SET myName = @Name, Ext1 = @Ext1 WHERE id = @w_id";

            Assert.Equal(exp, sql);
        }

        [Fact]
        public void UpdateTest()
        {
            var data = new UpdateDemo { Name = "cat", Ext1 = "ext1" };
            var con = new { id = 1 };
            var sql = DapperExt.Update(data, con, "myt");

            var exp = "UPDATE myt SET Name = @Name, Ext1 = @Ext1, Ext2 = @Ext2, Ext3 = @Ext3 WHERE id = @w_id";

            Assert.Equal(exp, sql);
        }

        [Fact]
        public void UpdateTest2()
        {
            var data = new UpdateDemo2 { Name = "cat", Ext1 = "ext1" };
            var con = new { id = 1 };
            var sql = DapperExt.Update(data, con, "myt");

            var exp = "UPDATE myt SET myName = @Name, Ext1 = @Ext1, Ext2 = @Ext2 WHERE id = @w_id";

            Assert.Equal(exp, sql);
        }

        [Fact]
        public void GetCountTest()
        {
            var con = new { id = 1 };
            var sql = DapperExt.GetCount(con, "myt");

            var exp = "SELECT COUNT(*) FROM myt WHERE id = @w_id";

            Assert.Equal(exp, sql);
        }

        [Fact]
        public void GetCountTest2()
        {
            var sql = DapperExt.GetCount("id = @w_id", new { w_id = 1 }, "myt");

            var exp = "SELECT COUNT(*) FROM myt WHERE id = @w_id";

            Assert.Equal(exp, sql);
        }

        [Fact]
        public void UpdateNotNullModelWithIdTest()
        {
            var data = new UpdateDemo3 { Id = 1, Name = "cat" };
            var con = new { id = 1 };
            var sql = DapperExt.UpdateNotNull(data, con, "myt");

            var exp = "UPDATE myt SET last_name = @Name WHERE id = @w_id";

            Assert.Equal(exp, sql);
        }

        [Fact]
        public void UpdateNotNullModelWithIdTest2()
        {
            var data = new UpdateDemo4 { Id = 1, Name = "cat" };
            var con = new { id = 1 };
            var sql = DapperExt.UpdateNotNull(data, con, "myt");

            var exp = "UPDATE myt SET a_id = @Id, last_name = @Name WHERE id = @w_id";

            Assert.Equal(exp, sql);
        }

        [Fact]
        public void UpdateModelWithIdTest()
        {
            var data = new UpdateDemo3 { Id = 1, Name = "cat" };
            var con = new { id = 1 };
            var sql = DapperExt.Update(data, con, "myt");

            var exp = "UPDATE myt SET last_name = @Name WHERE id = @w_id";

            Assert.Equal(exp, sql);
        }

        [Fact]
        public void UpdateModelWithIdTest2()
        {
            var data = new UpdateDemo4 { Id = 1, Name = "cat" };
            var con = new { id = 1 };
            var sql = DapperExt.Update(data, con, "myt");

            var exp = "UPDATE myt SET a_id = @Id, last_name = @Name WHERE id = @w_id";

            Assert.Equal(exp, sql);
        }
    }
}

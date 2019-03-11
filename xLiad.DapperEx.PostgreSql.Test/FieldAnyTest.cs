using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using xLiAd.DapperEx.RepositoryPg;
using Xunit;

namespace xLiad.DapperEx.PostgreSql.Test
{
    public class FieldAnyTest
    {
        string constring = "Host=127.0.0.1;Port=5432;Database=zhanglei;Username=postgres;Password=zhanglei";
        RepositoryPg<News2> RepoNews2 => new RepositoryPg<News2>(constring);
        [Fact]
        public void TestFieldAnyWhere()
        {
            var RepoNews2 = this.RepoNews2;
            var rst = RepoNews2.FieldAny<Author>(x => x.Author, x => x.Name.Contains("张")).Where(x => true);
            Assert.NotEmpty(rst);
            Assert.NotEmpty(rst.First().Author);
            Assert.Contains("张", rst.First().Author.First().Name);
            var rst2 = RepoNews2.FieldAny<Author>(x => x.Author, x => x.Name.Contains("张") && x.Id > 2).Where(x => true, x => x.Title);
            Assert.NotEmpty(rst2);
            Assert.Null(rst2.First().Author);
            Assert.NotEmpty(rst2.First().Title);
        }
        [Fact]
        public void TestFieldAnyWhereSelect()
        {
            var RepoNews2 = this.RepoNews2;
            var rst = RepoNews2.FieldAny<Author>(x => x.Author, x => x.Id > 3).WhereSelect(x => true, x => x.Title);
            Assert.NotEmpty(rst);
            Assert.NotNull(rst.First());
            var rst1 = RepoNews2.FieldAny<Author>(x => x.Author, x => x.Id > 3).WhereSelect(x => true, x => x.Author);
            Assert.NotEmpty(rst1);
            Assert.NotEmpty(rst1.First());
            var rst2 = RepoNews2.FieldAny<Author>(x => x.Author, x => x.Id > 3).WhereOrderSelect(x => x.Id > 1, x => x.Id, x => x.Author, 1);
            Assert.NotEmpty(rst2);
            Assert.NotEmpty(rst2.First());
        }
        [Fact]
        public void TestFieldAnyFind()
        {
            var RepoNews2 = this.RepoNews2;
            var rst = RepoNews2.FieldAny<Author>(x => x.Author, x => x.Id > 3).Find(x => x.Id == 11);
            var rst1 = RepoNews2.FieldAny<Author>(x => x.Author, x => x.Id > 3).Find(11);
            var rst2 = RepoNews2.FieldAny<Author>(x => x.Author, x => x.Id > 3).FindField(x => true, x => x.Title);
            Assert.Equal(rst.Title, rst1.Title);
            Assert.Equal(rst.Content, rst1.Content);
        }
        [Fact]
        public void TestFieldAnyCount()
        {
            var rst = RepoNews2.FieldAny<Author>(x => x.Author, x => x.BirthDay < DateTime.Today).Count(x => true);
            Assert.NotEqual(0, rst);
        }
        [Fact]
        public void TestFieldAnyAll()
        {
            var rst = RepoNews2.FieldAny<Author>(x => x.Author, x => x.Name.Contains("张") && x.Id > 2).All();
            Assert.NotEmpty(rst);
        }
        [Fact]
        public void TestFieldPageList()
        {
            var rst = RepoNews2.FieldAny<Author>(x => x.Author, x => x.Id > 3).PageList(x => x.Id > 0, x => x.Id, 1, 10);
            Assert.NotEmpty(rst.Items);
        }
    }
}

using System;
using System.Collections.Generic;
using System.Text;
using xLiAd.DapperEx.RepositoryPg;

namespace xLiad.DapperEx.PostgreSql.Test
{
    public class FieldAnyTest
    {
        string constring = "Host=127.0.0.1;Port=5432;Database=zhanglei;Username=postgres;Password=zhanglei";
        RepositoryPg<News2> RepoNews2 => new RepositoryPg<News2>(constring);

        public void TestFieldAnyWhere()
        {
            //还需要做的：把这里补充完整、把QueryTest 里的第一个弄好、把最后一个 samples 里的项目改到 Test 里来。
            //var l270 = RepoNews2.Where(x => x.Author.Contains("c"));
            //var ltsd1 = RepoNews2.FieldAny<Author>(x => x.Author, x => x.Name.Contains("张")).Where(x => true);
            //var ltsd2 = RepoNews2.FieldAny<Author>(x => x.Author, x => x.Name.Contains("张") && x.Id > 4).Where(x => true, x=>x.Title);
            //var ltsd3 = RepoNews2.FieldAny<Author>(x => x.Author, x => x.Id > 3).WhereSelect(x => true, x => x.Title);
            //var ltsd4 = RepoNews2.FieldAny<Author>(x => x.Author, x => x.BirthDay < DateTime.Today).Count(x => true);
            //var ltsd5 = RepoNews2.FieldAny<Author>(x => x.Author, x => x.Name.Contains("张") && x.Id > 4).All();
            //var ltsd6 = RepoNews2.FieldAny<Author>(x => x.Author, x => x.Id > 3).WhereSelect(x => true, x => x.Author);
            //var ltsd7 = RepoNews2.FieldAny<Author>(x => x.Author, x => x.Id > 3).WhereOrderSelect(x => x.Id > 1, x => x.Id, x=>x.Author, 1);
            //var ltsd8 = RepoNews2.FieldAny<Author>(x => x.Author, x => x.Id > 3).Find(x => x.Id == 1);
            //var ltsd9 = RepoNews2.FieldAny<Author>(x => x.Author, x => x.Id > 3).Find(1);
            //var ltsda = RepoNews2.FieldAny<Author>(x => x.Author, x => x.Id > 3).FindField(x=>true, x=>x.Title);
            //var ltsdb = RepoNews2.FieldAny<Author>(x => x.Author, x => x.Id > 3).PageList(x => x.Id > 0, x => x.Id, 1, 10);
        }
    }
}

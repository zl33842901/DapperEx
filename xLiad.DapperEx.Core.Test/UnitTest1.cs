using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using xLiAd.DapperEx.MsSql.Core;
using xLiAd.DapperEx.MsSql.Core.Core.Dialect;
using xLiAd.DapperEx.MsSql.Core.Helper;
using Xunit;

namespace xLiad.DapperEx.Core.Test
{
    public class UnitTest1
    {
        [Fact]
        public void Test1()
        {
            List<Expression<Func<TheModel, object>>> list = new List<Expression<Func<TheModel, object>>>();
            list.Add(x => x.Id);
            list.Add(x => x.Name);
            var epf = new ExpressionPropertyFinder(list[0], typeof(TheModel));
            var result = epf.MemberList;
        }
    }
}

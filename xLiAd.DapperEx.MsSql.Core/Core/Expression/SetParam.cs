using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace xLiAd.DapperEx.MsSql.Core.Core.Expression
{
    /// <summary>
    /// 给不继承 ExpressionVisitor 的类用
    /// </summary>
    public abstract class SetParam
    {
        private List<string> vs = new List<string>();
        protected string GetParamName(string oname)
        {
            var nname = oname;
            var i = 1;
            while (vs.Contains(nname))
            {
                nname = $"{oname}{i++}";
            }
            vs.Add(nname);
            return nname;
        }
    }
    /// <summary>
    /// 给继承 ExpressionVisitor 的类用。
    /// </summary>
    public abstract class SetParamVisitor : ExpressionVisitor
    {
        private List<string> vs = new List<string>();
        protected string GetParamName(string oname)
        {
            var nname = oname;
            var i = 1;
            while (vs.Contains(nname))
            {
                nname = $"{oname}{i++}";
            }
            vs.Add(nname);
            return nname;
        }
    }
}

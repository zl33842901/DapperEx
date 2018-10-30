using xLiAd.DapperEx.MsSql.Core.Core.Interfaces;
using xLiAd.DapperEx.MsSql.Core.Core.SetC;
using xLiAd.DapperEx.MsSql.Core.Core.SetQ;

namespace xLiAd.DapperEx.MsSql.Core.Model
{
    public class DataBaseContext<T>
    {
        public QuerySet<T> QuerySet => (QuerySet<T>)Set;

        public CommandSet<T> CommandSet => (CommandSet<T>)Set;

        public ISet<T> Set { get; internal set; }

        internal EOperateType OperateType { get; set; }
    }
}

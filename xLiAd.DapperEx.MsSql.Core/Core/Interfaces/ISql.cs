using Dapper;
using System;
using System.Collections.Generic;
using System.Text;

namespace xLiAd.DapperEx.MsSql.Core.Core.Interfaces
{
    public interface ISql
    {
        DynamicParameters Params { get; }
        string SqlString { get; }
    }
}

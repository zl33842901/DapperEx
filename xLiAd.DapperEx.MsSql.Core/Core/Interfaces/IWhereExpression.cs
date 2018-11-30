﻿using Dapper;
using System;
using System.Collections.Generic;
using System.Text;

namespace xLiAd.DapperEx.MsSql.Core.Core.Interfaces
{
    public interface IWhereExpression
    {
        string SqlCmd { get; }
        DynamicParameters Param { get; }
    }
}

using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using xLiAd.DapperEx.Repository;

namespace xLiAd.DapperEx.RepositoryPg
{
    public interface IRepositoryPg<T> : IRepository<T>
    {
        IRepositoryPg<T> FieldAny<TField>(Expression<Func<T, IList<TField>>> Field, Expression<Func<TField, bool>> Any);
    }
}

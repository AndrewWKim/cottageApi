using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using CottageApi.Core.Domain.Entities.Base;
using Microsoft.EntityFrameworkCore;

namespace CottageApi.Core.Extensions
{
    public static class QueryableExtensions
    {
        public static IQueryable<TEntity> Paging<TEntity>(this IQueryable<TEntity> source, int offset, int limit)
            where TEntity : IEntity
        {
            return source.Skip(offset * limit).Take(limit);
        }

        public static IQueryable<TEntity> Paging<TEntity>(this IQueryable<TEntity> source, int? offset, int? limit)
            where TEntity : IEntity
        {
            return offset.HasValue && limit.HasValue ? source.Paging(offset.Value, limit.Value) : source;
        }

        public static Task<TSource> FindByIdAsync<TSource>(this IQueryable<TSource> source, int id, CancellationToken cancellationToken = default(CancellationToken))
            where TSource : BaseEntity
        {
            return source.SingleOrDefaultAsync(entity => entity.Id == id, cancellationToken);
        }

        public static IQueryable<TEntity> Include<TEntity>(this IQueryable<TEntity> query, IEnumerable<Expression<Func<TEntity, object>>> includeProperties)
            where TEntity : class
        {
            if (includeProperties == null)
            {
                return query;
            }

            foreach (var includeProperty in includeProperties)
            {
                query = query.Include(includeProperty);
            }

            return query.OfType<TEntity>();
        }

        public static IQueryable<T> OrderByCondition<T>(this IQueryable<T> query, Expression<Func<T, object>> expression, bool descending = false)
        {
            return !descending ? query.OrderBy(expression) : query.OrderByDescending(expression);
        }
    }
}

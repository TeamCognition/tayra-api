using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Cog.Core;
using LinqKit;
using Microsoft.EntityFrameworkCore;

namespace Cog.DAL
{
    /// <summary>
    ///     Provides set of extension methods to IQueryable interface.
    /// </summary>
    public static class IQueryableExtensions
    {
        /// <summary>
        ///     Converts List of conditional expressions to AND/OR chain.
        /// </summary>
        /// <typeparam name="T">Type of entity query is built against.</typeparam>
        /// <param name="queryable">Queryable entity source.</param>
        /// <param name="conditions">List of conditional expressions.</param>
        /// <param name="type">Type of chain.</param>
        /// <returns>Modified query.</returns>
        public static IQueryable<T> Chain<T>(this IQueryable<T> queryable, ChainType type,
            params Expression<Func<T, bool>>[] conditions)
        {
            Expression<Func<T, bool>> expression;
            if (type == ChainType.AND)
            {
                if (conditions != null)
                {
                    expression = PredicateBuilder.New<T>(x => true);
                    foreach (var condition in conditions) expression = expression.And(condition);
                    queryable = queryable.Where(expression.Expand());
                }
            }
            else if (conditions != null)
            {
                expression = PredicateBuilder.New<T>(x => false);
                foreach (var condition in conditions) expression = expression.Or(condition);
                queryable = queryable.Where(expression.Expand());
            }

            return queryable;
        }

        /// <summary>
        ///     Returns GridData container of DTO objects for specified grid parameters.
        /// </summary>
        /// <typeparam name="T">Type of DTO object.</typeparam>
        /// <param name="query">Search query to execute.</param>
        /// <param name="gridParams">Grid parameters.</param>
        /// <param name="orderModifier">Modifier for search order.</param>
        /// <returns>GridData container.</returns>
        public static async Task<GridData<T>> GetGridDataAsync<T>(this IQueryable<T> query, GridParams gridParams,
            string orderModifier = "") where T : class
        {
            query = BuildGridDataQuery(query, gridParams);

            return new GridData<T>
            {
                Records = await query.GetRecordsAsync(gridParams, orderModifier).ConfigureAwait(false),
                Total = await query.CountAsync().ConfigureAwait(false)
            };
        }

        /// <summary>
        ///     Returns GridData container of DTO objects for specified grid parameters.
        /// </summary>
        /// <typeparam name="T">Type of DTO object.</typeparam>
        /// <param name="query">Search query to execute.</param>
        /// <param name="gridParams">Grid parameters.</param>
        /// <param name="orderModifier">Modifier for search order.</param>
        /// <returns>GridData container.</returns>
        public static GridData<T> GetGridData<T>(this IQueryable<T> query, GridParams gridParams,
            string orderModifier = "") where T : class
        {
            query = BuildGridDataQuery(query, gridParams);

            return new GridData<T>
            {
                Records = query.GetRecords(gridParams, orderModifier),
                Total = query.Count()
            };
        }

        /// <summary>
        ///     Returns list of records based on provided query definition and gridParams.
        /// </summary>
        /// <typeparam name="T">Type of record to expect.</typeparam>
        /// <param name="query">Source query.</param>
        /// <param name="gridParams">Grid parameters.</param>
        /// <param name="orderModifier">Additional sort parameters.</param>
        /// <returns>List of records.</returns>
        public static async Task<List<T>> GetRecordsAsync<T>(this IQueryable<T> query, GridParams gridParams,
            string orderModifier = "") where T : class
        {
            return await query.OrderBy(gridParams.Ordering() + orderModifier)
                .Skip((gridParams.Page - 1) * gridParams.Rows).Take(gridParams.Rows).ToListAsync();
        }

        /// <summary>
        ///     Returns list of records based on provided query definition and gridParams.
        /// </summary>
        /// <typeparam name="T">Type of record to expect.</typeparam>
        /// <param name="query">Source query.</param>
        /// <param name="gridParams">Grid parameters.</param>
        /// <param name="orderModifier">Additional sort parameters.</param>
        /// <returns>List of records.</returns>
        public static List<T> GetRecords<T>(this IQueryable<T> query, GridParams gridParams, string orderModifier = "")
            where T : class
        {
            return query.OrderBy(gridParams.Ordering() + orderModifier).Skip((gridParams.Page - 1) * gridParams.Rows)
                .Take(gridParams.Rows).ToList();
        }

        #region Private Methods

        private static IQueryable<T> BuildGridDataQuery<T>(IQueryable<T> query, GridParams gridParams) where T : class
        {
            if (!gridParams.IsValidPropertyName<T>()) throw new CogSecurityException("sql injection attempt");

            if (gridParams.Filters != null)
                foreach (var f in gridParams.Filters)
                {
                    if (f.Values == null || f.Values.Length == 0)
                        continue;

                    switch (f.Type)
                    {
                        case GridParams.Filter.FilterTypes.Match:
                            query = query.Where($"{f.Idx} in ({string.Join(',', f.Values)})");
                            break;

                        case GridParams.Filter.FilterTypes.Range:
                            query = query.Where($"{f.Idx} >= @0 AND {f.Idx} <= @1", f.Values[0], f.Values[1]);
                            break;

                        case GridParams.Filter.FilterTypes.Like:
                            foreach (var str in f.Values) query = query.Where($"{f.Idx}.Contains(@0)", str);
                            break;
                    }
                }

            return query;
        }

        #endregion
    }
}
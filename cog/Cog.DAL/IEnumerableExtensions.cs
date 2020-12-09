using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using Cog.Core;

namespace Cog.DAL
{
    /// <summary>
    ///     Provides set of extension methods to IEnumerable interface.
    /// </summary>
    public static class IEnumerableExtensions
    {
        /// <summary>
        ///     Returns GridData container for specified collection and provided gridParams.
        /// </summary>
        /// <typeparam name="T">Type of record.</typeparam>
        /// <param name="collection">Collection of items.</param>
        /// <param name="gridParams">Grid parameters.</param>
        /// <param name="orderModifier">Sort modifier.</param>
        /// <returns>GridData container of records.</returns>
        public static GridData<T> GetGridData<T>(this IEnumerable<T> collection, GridParams gridParams,
            string orderModifier = "") where T : class
        {
            return new()
            {
                Records = collection.GetRecords(gridParams, orderModifier),
                Total = collection.Count()
            };
        }

        /// <summary>
        ///     Returns list of records from the collection based on the provided gridParams.
        /// </summary>
        /// <typeparam name="T">Type of record.</typeparam>
        /// <param name="collection">Source query.</param>
        /// <param name="gridParams">Grid parameters.</param>
        /// <param name="orderModifier">Additional sort parameters.</param>
        /// <returns>List of records.</returns>
        public static List<T> GetRecords<T>(this IEnumerable<T> collection, GridParams gridParams,
            string orderModifier = "")
        {
            return collection.AsQueryable().OrderBy(gridParams.Ordering() + orderModifier)
                .Skip((gridParams.Page - 1) * gridParams.Rows).Take(gridParams.Rows).ToList();
        }
    }
}
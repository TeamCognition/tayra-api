using System;
using System.Linq;
using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;

namespace Cog.DAL
{
    public static class ModelBuilderExtensions
    {
        /// <summary>
        /// Applying query filters to entities which implement a particular interface
        /// </summary>
        /// <param name="modelBuilder"></param>
        /// <param name="expression"></param>
        /// <example>ApplyGlobalFilters<IDeleted/>(e => !e.IsDeleted);</example>
        /// <typeparam name="TInterface"></typeparam>
        public static void ApplyGlobalFilters<TInterface>(this ModelBuilder modelBuilder,
            Expression<Func<TInterface, bool>> expression)
        {
            foreach (var entityType in modelBuilder.Model.GetEntityTypes())
            {
                if (entityType.ClrType.GetInterface(typeof(TInterface).Name) != null)
                {
                    var newParam = Expression.Parameter(entityType.ClrType);
                    var newBody = ReplacingExpressionVisitor.Replace(expression.Parameters.Single(), newParam, expression.Body);
                    modelBuilder.Entity(entityType.ClrType).HasQueryFilter(Expression.Lambda(newBody, newParam));
                }
            }
        }
        
        /// <summary>
        /// Applying query filters to entities with a particular column name

        /// </summary>
        /// <param name="modelBuilder"></param>
        /// <param name="propertyName"></param>
        /// <param name="value"></param>
        /// <example>ApplyGlobalFilters<bool/>("IsDeleted", false);</example>
        /// <typeparam name="T"></typeparam>
        public static void ApplyGlobalFilters<T>(this ModelBuilder modelBuilder,
            string propertyName, T value)
        {
            foreach (var entityType in modelBuilder.Model.GetEntityTypes())
            {
                var foundProperty = entityType.FindProperty(propertyName);
                if (foundProperty == null || foundProperty.ClrType != typeof(T)) continue;
                var newParam = Expression.Parameter(entityType.ClrType);
                var filter = Expression.
                    Lambda(Expression.Equal(Expression.Property(newParam, propertyName),
                        Expression.Constant(value)), newParam);

                modelBuilder.Entity(entityType.ClrType).HasQueryFilter(filter);
            }
        }
    }
}
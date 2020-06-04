using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace Cog.Core
{
    public static class ExpressionExtensions
    {
        public static string GetPropertyPath<TClass, TProperty>(this Expression<Func<TClass, TProperty>> propertySelector)
        {
            MemberExpression memberExpression = propertySelector.Body as MemberExpression;
            List<string> list = new List<string>();
            while (memberExpression != null)
            {
                list.Add(memberExpression.Member.Name);
                memberExpression = (memberExpression.Expression as MemberExpression);
            }
            list.Reverse();
            return string.Join(".", list);
        }
    }
}

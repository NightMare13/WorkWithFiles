using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace WorkWithFiles.Utility
{
    internal static class ExpressionExtensions
    {
        public static string ToName<T>(this Expression<Func<T>> expression)
        {
            var propertyExpression = expression.Body as MemberExpression;
            var property = propertyExpression?.Member as PropertyInfo;

            return property?.Name;
        }
    }
}

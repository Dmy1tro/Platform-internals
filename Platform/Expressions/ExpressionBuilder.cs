using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace HighPerformance
{
    public class ExpressionBuilder
    {
        public static Expression<Func<string, string, string>> Concat()
        {
            var arg1 = Expression.Parameter(typeof(string), "s1");
            var arg2 = Expression.Parameter(typeof(string), "s2");

            var arg1CheckNull = Expression.NotEqual(arg1, Expression.Constant(null));
            var arg1Safe = Expression.Condition(arg1CheckNull, arg1, Expression.Constant(string.Empty, typeof(string)));

            var arg2CheckNull = Expression.NotEqual(arg2, Expression.Constant(null));
            var arg2Safe = Expression.Condition(arg2CheckNull, arg2, Expression.Constant(string.Empty, typeof(string)));

            var method = typeof(string).GetMethod(nameof(string.Concat), new[] { typeof(string), typeof(string) });

            var concat = Expression.Call(method, arg1Safe, arg2Safe);

            return Expression.Lambda<Func<string, string, string>>(concat, new[] { arg1, arg2 });
        }

        public static Expression<Func<IEnumerable<T>, int>> ItemsCount<T>()
        {
            var arg1 = Expression.Parameter(typeof(IEnumerable<T>), "items");
            var itemType = arg1.Type.GetGenericArguments()[0];

            var method = Expression.Call(
                typeof(Enumerable),
                nameof(Enumerable.Count),
                new[] { itemType },
                new[] { arg1 });

            return Expression.Lambda<Func<IEnumerable<T>, int>>(method, arg1);
        }

        public static Expression<Func<IEnumerable<T>, int>> ItemsCountWhereItemIsNotDefault<T>()
        {
            // items = [1,2,3,4,5]
            var arg1 = Expression.Parameter(typeof(IEnumerable<T>), "items");
            var itemType = arg1.Type.GetGenericArguments()[0];

            // x => x != default
            var lambdaVariable = Expression.Parameter(typeof(T), "x");
            var condition = Expression.NotEqual(lambdaVariable, Expression.Default(typeof(T)));
            var lambdaBody = (Expression)Expression.Lambda(condition, lambdaVariable);

            // items.Count(x => x != default)
            var method = Expression.Call(
                typeof(Enumerable),
                nameof(Enumerable.Count),
                new[] { itemType },
                new[] { arg1, lambdaBody });

            return Expression.Lambda<Func<IEnumerable<T>, int>>(method, arg1);
        }

        public static Expression<Func<IEnumerable<T>, List<T>>> Filter<T>(Expression<Func<T, bool>> predicate)
        {
            var items = Expression.Parameter(typeof(IEnumerable<T>), "items");
            var itemType = items.Type.GetGenericArguments()[0];

            var whereMethod = Expression.Call(
                typeof(Enumerable),
                nameof(Enumerable.Where),
                new[] { itemType },
                new[] { items, (Expression)predicate });

            var toListMethod = Expression.Call(
                typeof(Enumerable),
                nameof(Enumerable.ToList),
                new[] { itemType },
                new[] { whereMethod });

            return Expression.Lambda<Func<IEnumerable<T>, List<T>>>(toListMethod, items);
        }
    }
}

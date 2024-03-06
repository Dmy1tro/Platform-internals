using System;
using System.Threading.Tasks;
using BenchmarkDotNet.Running;

namespace HighPerformance
{
    class Program
    {
        static void Main(string[] args)
        {
            // BenchmarkRunner.Run<Bench>();
            var lambda = ExpressionBuilder.Concat();

            var concat = lambda.Compile();

            var res = concat("1", "2");

            //var countLambda = ExpressionBuilder.Filter();
            //var count = countLambda.Compile();

            //var res2 = count(new[] {1,2,3,4,5,0,0,0});

            //TestExpressions();
        }

       // Span not working in async
       //public async Task TestSpanInAsync(Span<int> input)
       //{
       //     // ReadOnlySpan<char> s = "qwerty".AsSpan();

       //     // Span not working in lambda
       //     var span = "qwerty".AsSpan();
       //     Action act = () =>
       //     {
       //         span.Slice(0, 1);
       //     };
       //}

        public static void TestExpressions()
        {
            var concatLambda = ExpressionBuilder.Concat();
            var concat = concatLambda.Compile();
            var res1 = concat.Invoke("q1", " q2");

            var countLambda = ExpressionBuilder.ItemsCount<int>();
            var count = countLambda.Compile();
            var res2 = count.Invoke(new[] { 1, 2, 3, 4, 5 });

            var countWithoutDefaultsLabmda = ExpressionBuilder.ItemsCountWhereItemIsNotDefault<int>();
            var countWithoutDefaults = countWithoutDefaultsLabmda.Compile();
            var res3 = countWithoutDefaults.Invoke(new[] { 0,1,2,3,4,5,0 });

            var filterLambda = ExpressionBuilder.Filter<int>(i => i % 2 == 0);
            var filter = filterLambda.Compile();
            var res4 = filter.Invoke(new[] { 0,1,2,3,4,5,6,7,8,9 });
        }
    }
}

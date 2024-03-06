using System;

namespace Strings
{
    class DateParser
    {
        public static DateTime Parse(string date)
        {
            var year = date.Substring(0, 4);
            var month = date.Substring(5, 2);
            var day = date.Substring(8, 2);

            return new DateTime(int.Parse(year), int.Parse(month), int.Parse(day));
        }

        public static DateTime ParseOptimized(string date)
        {
            ReadOnlySpan<char> dateSpan = date;

            var year = dateSpan.Slice(0, 4);
            var month = dateSpan.Slice(5, 2);
            var day = dateSpan.Slice(8, 2);

            return new DateTime(int.Parse(year), int.Parse(month), int.Parse(day));
        }

        public static DateTime ParseWithStackAllocation(string date)
        {
            ReadOnlySpan<char> dateSpan = date;

            Span<char> year = stackalloc char[4];
            Span<char> month = stackalloc char[2];
            Span<char> day = stackalloc char[2];

            year[0] = dateSpan[0];
            year[1] = dateSpan[1];
            year[2] = dateSpan[2];
            year[3] = dateSpan[3];

            month[0] = dateSpan[5];
            month[1] = dateSpan[6];

            day[0] = dateSpan[8];
            day[1] = dateSpan[9];

            return new DateTime(int.Parse(year), int.Parse(month), int.Parse(day));
        }
    }
}

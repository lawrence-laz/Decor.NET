using System;
using System.Collections.Generic;

namespace Decor.Internal
{
    internal static class StringExtensions
    {
        public static string AsParam(this string name) => $" decor_{name}";

        public static string JoinStrings(this IEnumerable<string> values, string separator) => string.Join(separator, values);

        public static string ModifyString(this string value, Func<string, string> modifier) => modifier(value);
    }
}

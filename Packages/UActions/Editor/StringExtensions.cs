using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace UActions.Editor
{
    public static class StringExtensions
    {
        // \{(.*?)\} -> {value}
        // (?<=\{).*?(?=\}) -> value : require positive lookbehind to regex engine
#if UNITY_2021_2_OR_NEWER
        private static readonly Regex CurlyBracketsPattern = new(@"\$\((.*?)\)", RegexOptions.Multiline);
#else
        private static readonly Regex CurlyBracketsPattern = new Regex(@"\$\((.*?)\)", RegexOptions.Multiline);
#endif
        public static string Format(this string format, IReadOnlyDictionary<string, string> dictionary)
        {
#if UNITY_2021_2_OR_NEWER
            return CurlyBracketsPattern.Replace(format,
                match => dictionary.TryGetValue(match.Value[2..^1], out var value) ? value : match.Value);
#else
            return CurlyBracketsPattern.Replace(format,
                match => dictionary.TryGetValue(match.Value.Substring(2, match.Value.Length - 3), out var value) ? value : match.Value);
#endif
        }

        public static string PascalToKebabCase(this string value)
        {
            if (string.IsNullOrEmpty(value))
                return value;

            return Regex.Replace(
                    value,
                    "(?<!^)([A-Z][a-z]|(?<=[a-z])[A-Z])",
                    "-$1",
                    RegexOptions.Compiled)
                .Trim()
                .ToLower();
        }
    }
}
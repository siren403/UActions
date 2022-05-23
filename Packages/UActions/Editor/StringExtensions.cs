using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace UActions.Editor
{
    public static class StringExtensions
    {
        // \{(.*?)\} -> {value}
        // (?<=\{).*?(?=\}) -> value : require positive lookbehind to regex engine
        private static readonly Regex CurlyBracketsPattern = new(@"\$\((.*?)\)", RegexOptions.Multiline);

        public static string Format(this string format, IReadOnlyDictionary<string, string> dictionary)
        {
            return CurlyBracketsPattern.Replace(format,
                match => dictionary.TryGetValue(match.Value[2..^1], out var value) ? value : match.Value);
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
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace GapFillUtility.Services.Parsers
{
    public static class EntityParser
    {
        //private static readonly Regex EntityParserRegex = new Regex(@"^[Ee]\s+(?<key>[\w\d-:]+)\s+(?<label>[\w\d]+)(?:\s+(?<outKey>[\w\d:]+)\s+(?<outLabel>[\w\d:]+)\s+(?<inKey>[\w\d:]+)\s+(?<inLabel>[\w\d:]+))?.*|^[Vv]\s+(?<key>[\w\d-:]+)\s+(?<label>[\w\d]+).*", RegexOptions.IgnoreCase | RegexOptions.Singleline | RegexOptions.Compiled);
        private static readonly Regex EntityParserRegex = new Regex(@"^[Ee]\s+(?<key>[\w\d-:]+)\s+(?<label>[\w\d]+)(?:\s+(?<outKey>[\w\d-:]+)\s+(?<outLabel>[\w\d:]+)\s+(?<inKey>[\w\d-:]+)\s+(?<inLabel>[\w\d:]+))?.*|^[Vv]\s+(?<key>[\w\d-:]+)\s+(?<label>[\w\d]+).*", RegexOptions.IgnoreCase | RegexOptions.Singleline | RegexOptions.Compiled);
        private static readonly Regex PropertiesParserRegex = new Regex(@"(?:\s*{\s*""(?<propertyName>.*?)""\s*,\s*""(?<propertyValue>.*?)""\s*},?)", RegexOptions.IgnoreCase | RegexOptions.Singleline | RegexOptions.Compiled);

        public static string ReadKey(string input)
        {
            return EntityParserRegex.Match(input).Groups["key"].Value;
        }

        public static string ReadLabel(string input)
        {
            return EntityParserRegex.Match(input).Groups["label"].Value;
        }

        public static string ReadInKey(string input)
        {
            return EntityParserRegex.Match(input).Groups["inKey"].Value;
        }

        public static string ReadOutKey(string input)
        {
            return EntityParserRegex.Match(input).Groups["outKey"].Value;
        }

        public static Dictionary<string, string> ReadProperties(string input)
        {
            var result = new Dictionary<string, string>();
            Match matchResult = PropertiesParserRegex.Match(input);
            while (matchResult.Success)
            {
                var propertyName = matchResult.Groups["propertyName"].Value;
                var normalizedPropertyName = NormalizeName(propertyName);
                result.Add(normalizedPropertyName,
                           matchResult.Groups["propertyValue"].Value);
                matchResult = matchResult.NextMatch();
            }

            return result;
        }

        // TODO: Fix in xslt instead
        // Will transform some-name into someName (tire separated into camelCase)
        private static string NormalizeName(string propertyName)
        {
            var sb = new StringBuilder(propertyName.Length);
            var capitalMode = false;

            for (int i = 0; i < propertyName.Length; i++)
            {
                if (propertyName[i] == '-')
                {
                    capitalMode = true;
                }
                else
                {
                    if (capitalMode)
                    {
                        sb.Append(char.ToUpper(propertyName[i]));
                        capitalMode = false;
                    }
                    else
                    {
                        sb.Append(propertyName[i]);
                    }
                }
            }

            return sb.ToString();
        }
    }
}

using System.Linq;

namespace Max.BotClient
{
    internal static class BotClientExtensions
    {
        public static string ToSnakeCase(this string str) =>
            string.Concat(str.Select((c, i) => i > 0 && char.IsUpper(c) ? "_" + c : c.ToString())).ToLower();
    }
}


using System.Text;

namespace YogurtTheBot.Game.Core.Helpers
{
    public static class StringExtensions
    {
        public static string ToSnakeCase(this string s)
        {
            if (string.IsNullOrEmpty(s))
            {
                return s;
            }

            var res = new StringBuilder();

            foreach (char c in s)
            {
                res.Append(
                    char.IsUpper(c) && res.Length > 0
                        ? "_" + char.ToLower(c)
                        : char.ToLower(c).ToString()
                );
            }

            return res.ToString();
        }
    }
}
namespace Unbiased.Shared.Extensions.Concrete.Helpers
{
    public static class SecurityExtensions
    {
        public static string SanitizeForSql(this string input)
        {
            var blockList = new string[] { "--", ";--", ";", "/*", "*/", "@@", "@" };
            foreach (var blockItem in blockList)
            {
                if (input.Contains(blockItem))
                {
                    throw new InvalidOperationException("Potentially dangerous input detected.");
                }
            }
            return input;
        }
    }
}

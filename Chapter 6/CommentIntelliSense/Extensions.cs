using System;

namespace CommentIntellisense
{
    internal static class Extensions
    {
        internal const string Trigger = "// RV";

        public static bool HasRvTrigger(this string text) => !string.IsNullOrWhiteSpace(text) &&
                   text.IndexOf(Trigger, StringComparison.OrdinalIgnoreCase) >= 0;       
    }
}

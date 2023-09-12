namespace AmazonWebServices.Utilities
{
    internal static class TextTool
    {
        public static string ClearSpecialCharacters(this string text)
        {
            text = text
                .ToLower()
                .Trim()
                .Replace("/", "-")
                .Replace("\"", "")
                .Replace("=", "")
                .Replace("!", "")
                .Replace("?", "")
                .Replace("(", "")
                .Replace(")", "")
                .Replace("'", "")
                .Replace(",", "-")
                .Replace("’", "-")
                .Replace("’", "-")
                .Replace("*", "")
                .Replace(" ", "-")
                .Replace("+", "-")
                .Replace(".", "-")
                .Replace("_", "-")
                .Replace("&", "-")
                .Replace("[", "")
                .Replace("]", "")
                .Replace("|", "-")
                .Replace("%", "-")
                .Replace("–", "-")//Although the Replace character looks like a hyphen, it is not :)

                .Replace("ü", "u")
                .Replace("ı", "i")
                .Replace("ö", "o")
                .Replace("ü", "u")
                .Replace("ş", "s")
                .Replace("ğ", "g")
                .Replace("ç", "c")

                .Replace("Ü", "u")
                .Replace("İ", "i")
                .Replace("Ö", "o")
                .Replace("Ü", "u")
                .Replace("Ş", "s")
                .Replace("Ğ", "g")
                .Replace("Ç", "c")

                .Replace("Ầ", "A")
                .Replace("Ẩ", "A")

                .Replace("è", "e")
                .Replace("é", "e")
                .Replace("ê", "e")
                .Replace("â", "a")
                .Replace("ầ", "a")
                .Replace("ẩ", "a")
                .Replace("ẫ", "a")
                .Replace("ấ", "a")
                .Replace("ậ", "a")
                .Replace("î", "i")
                .Replace("ô", "o")
                .Replace("û", "u")
                .Replace("ž", "z")
                .Replace("ć", "c")

                .Replace("✦", "-")

                .Replace("----------", "-")
                .Replace("---------", "-")
                .Replace("--------", "-")
                .Replace("-------", "-")
                .Replace("------", "-")
                .Replace("-----", "-")
                .Replace("----", "-")
                .Replace("---", "-")
                .Replace("--", "-")

                .Trim('-');

            return text;
        }
    }
}

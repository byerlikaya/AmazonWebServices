namespace AmazonWebServices.Utilities;

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
           .Replace(",", "")
           .Replace("’", "")
           .Replace("ʼ", "")
           .Replace("ʻ", "")
           .Replace("‘", "")
           .Replace("ʹ", "")
           .Replace("*", "")
           .Replace(" ", "-")
           .Replace("\u00a0", "-")
           .Replace("+", "-")
           .Replace(".", "-")
           .Replace("_", "-")
           .Replace("&", "-")
           .Replace("[", "")
           .Replace("]", "")
           .Replace("|", "-")
           .Replace("%", "-")
           .Replace("–", "-")
           .Replace("ʹ", "")

           .Replace("Ầ", "A")
           .Replace("Ẩ", "A")
           .Replace("Ü", "u")
           .Replace("İ", "i")
           .Replace("Ö", "o")
           .Replace("Ō", "o")
           .Replace("Ü", "u")
           .Replace("Ş", "s")
           .Replace("Ğ", "g")
           .Replace("Ç", "c")

           .Replace("ğ", "g")

           .Replace("ž", "z")

           .Replace("κ", "k")

           .Replace("ń", "n")

           .Replace("ł", "l")

           .Replace("ı", "i")
           .Replace("î", "i")
           .Replace("í", "i")

           .Replace("ö", "o")
           .Replace("ō", "o")
           .Replace("ô", "o")

           .Replace("ü", "u")
           .Replace("û", "u")
           .Replace("ŭ", "u")
           .Replace("ú", "u")

           .Replace("ş", "s")
           .Replace("š", "s")
           .Replace("ś", "s")
           .Replace("ș", "s")

           .Replace("ç", "c")
           .Replace("ć", "c")
           .Replace("č", "c")
           .Replace("с", "c")

           .Replace("è", "e")
           .Replace("é", "e")
           .Replace("ê", "e")
           .Replace("ə", "e")

           .Replace("â", "a")
           .Replace("ầ", "a")
           .Replace("ẩ", "a")
           .Replace("ẫ", "a")
           .Replace("ấ", "a")
           .Replace("ậ", "a")
           .Replace("ā", "a")
           .Replace("ă", "a")
           .Replace("á", "a")

           .Replace("✦", "-")

           .Replace("-–-", "-")
           .Replace("–", "-")

           .Replace("--", "-")
           .Replace("---", "-")
           .Replace("----", "-")
           .Replace("-----", "-")
           .Replace("------", "-")
           .Replace("-------", "-")
           .Replace("--------", "-")
           .Replace("---------", "-")
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
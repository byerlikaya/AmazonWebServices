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
         .Replace("»", "-")
         .Replace("\u00ae", "-")
         .Replace("\u00be", "3-4")

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

         .Replace("â", "a")
         .Replace("ầ", "a")
         .Replace("ẩ", "a")
         .Replace("ẫ", "a")
         .Replace("ấ", "a")
         .Replace("ậ", "a")
         .Replace("ā", "a")
         .Replace("ă", "a")
         .Replace("á", "a")
         .Replace("ã", "a")
         .Replace("å", "a")
         .Replace("à", "a")
         .Replace("ä", "a")

         .Replace("ß", "b")

         .Replace("ç", "c")
         .Replace("ć", "c")
         .Replace("č", "c")
         .Replace("с", "c")

         .Replace("è", "e")
         .Replace("é", "e")
         .Replace("ê", "e")
         .Replace("ə", "e")

         .Replace("ğ", "g")

         .Replace("ı", "i")
         .Replace("î", "i")
         .Replace("í", "i")
         .Replace("ì", "i")
         .Replace("ï", "i")

         .Replace("κ", "k")

         .Replace("ł", "l")

         .Replace("ń", "n")
         .Replace("ñ", "n")

         .Replace("ş", "s")
         .Replace("š", "s")
         .Replace("ś", "s")
         .Replace("ș", "s")

         .Replace("ö", "o")
         .Replace("ō", "o")
         .Replace("ô", "o")
         .Replace("ó", "o")
         .Replace("ø", "o")
         .Replace("õ", "o")
         .Replace("º", "o")

         .Replace("ü", "u")
         .Replace("û", "u")
         .Replace("ŭ", "u")
         .Replace("ú", "u")
         .Replace("ù", "u")

         .Replace("ž", "z")

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
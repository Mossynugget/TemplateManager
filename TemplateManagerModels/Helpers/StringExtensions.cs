namespace TemplateManagerModels.Models.Helpers;

using System.Text;

public static class StringExtensions
{
  public static string AddSpacesToSentence(this string text, bool preserveAcronyms = false)
  {
    if (string.IsNullOrWhiteSpace(text))
      return string.Empty;
    StringBuilder newText = new StringBuilder(text.Length * 2);
    newText.Append(text[0]);
    for (int i = 1; i < text.Length; i++)
    {
      if (char.IsUpper(text[i]))
        if ((text[i - 1] != ' ' && !char.IsUpper(text[i - 1])) ||
            (preserveAcronyms && char.IsUpper(text[i - 1]) &&
             i < text.Length - 1 && !char.IsUpper(text[i + 1])))
          newText.Append(' ');
      newText.Append(text[i]);
    }
    return newText.ToString();
  }

  public static string FirstCharToLowerCase(this string str)
  {
    if (string.IsNullOrEmpty(str) || char.IsLower(str[0]))
      return str;

    return char.ToLower(str[0]) + str.Substring(1);
  }

  public static string RemoveMultiLineStringEscapeCharacters(this string contents)
  {
    return contents.Replace("\r\n", "");
  }
}
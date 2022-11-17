namespace TemplateManagerModels.Models.Helpers;

using Newtonsoft.Json.Linq;
using System.Reflection.PortableExecutable;
using System.Runtime.CompilerServices;
using System.Text;

public static class VariableExtensions
{
  public static string KeyComment(this string key)
  {
    return key.KeyDerivative("comment");
  }

  public static string ValueComment(this string? value)
  {
    return value?.AddSpacesToSentence() ?? string.Empty;
  }

  public static string KeyUppercaseUnderscore(this string key)
  {
    return key.KeyDerivative("uppercaseUnderscore");
  }

  public static string ValueUppercaseUnderscore(this string? value)
  {
    return value?.AddUnderscoresToSentence().ToUpper() ?? string.Empty;
  }

  public static string KeyUnderscore(this string key)
  {
    return key.KeyDerivative("underscore");
  }

  public static string ValueUnderscore(this string? value)
  {
    return value?.AddUnderscoresToSentence() ?? string.Empty;
  }

  public static string KeyLowercase(this string key)
  {
    return key.KeyDerivative("lowercase");
  }

  public static string ValueLowercase(this string? value)
  {
    return value?.ToLower() ?? string.Empty;
  }

  public static string KeyDerivative(this string key, string identifier)
  {
    return $"{key.TrimEnd('$')}:{identifier}$";
  }
}
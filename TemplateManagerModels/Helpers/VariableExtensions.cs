namespace TemplateManagerModels.Models.Helpers;

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

  public static string KeyCamelCase(this string key)
  {
    return key.KeyDerivative("camelCase");
  }

  public static string ValueCamelCase(this string? value)
  {
    if (string.IsNullOrEmpty(value)) 
      return string.Empty;

    return char.ToLower(value[0]) + value.Substring(1);
  }

  public static string KeyLowercaseDashes(this string key)
  {
    return key.KeyDerivative("lowercaseDashes");
  }

  public static string ValueLowercaseDashes(this string? value)
  {
    return value?.AddDashesToSentence().ToLower() ?? string.Empty;
  }

  public static string KeyDerivative(this string key, string identifier)
  {
    return $"{key.TrimEnd('$')}:{identifier}$";
  }
}
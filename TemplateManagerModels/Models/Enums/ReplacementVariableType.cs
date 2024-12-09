namespace TemplateManager.Models.Enums;

public enum ReplacementVariableType
{
  /// <summary>
  /// A type denoting a value to be replaced in the file.
  /// </summary>
  /// <example>$RequestType$</example>
  Value,

  /// <summary>
  /// A type denoting a segment of code asking whether or not to show the code within.
  /// </summary>
  /// <example>$if:ContainsExample$example code$endif:ContainsExample$</example>
  If,

  /// <summary>
  /// A type denoting a segment of code asking whether or not to show the code within.
  /// </summary>
  /// <example>$select:ContractType:Query|Request$</example>
  Select,
}

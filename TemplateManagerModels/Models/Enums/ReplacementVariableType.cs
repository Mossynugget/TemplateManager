namespace TemplateManagerModels.Models.Enums;

internal enum ReplacementVariableType
{
  /// <summary>
  /// A type denoting a value to be replaced in the file.
  /// </summary>
  Value,

  /// <summary>
  /// A type denoting a segment of code asking whether or not to show the code within.
  /// </summary>
  If,
}

namespace TemplateManagerModels.Models.Enums;

public record ReplacementSettingType
{
  /// <summary>
  /// Used to determine the namespace from the string provided.
  /// </summary>
  public const string Namespace = "$setting:namespace$";

  /// <summary>
  /// Used to determine the solution path from the string provided.
  /// </summary>
  public const string SolutionPath = "$setting:solutionPath$";

  /// <summary>
  /// Used to determine the solution path from the string provided.
  /// </summary>
  public const string Solution = "$setting:solution$";

  /// <summary>
  /// Used to determine the project from the string provided.
  /// </summary>
  public const string ProjectName = "$setting:projectName$";
}

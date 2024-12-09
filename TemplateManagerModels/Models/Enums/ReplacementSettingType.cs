namespace TemplateManager.Models.Enums;

public record ReplacementSettingType
{
  /// <summary>
  /// Used to determine the namespace from the destination of the file.
  /// </summary>
  public const string Namespace = "$setting:namespace$";

  /// <summary>
  /// Used to determine the solution path from the source of the template exection.
  /// </summary>
  public const string SolutionPath = "$setting:solutionPath$";

  /// <summary>
  /// Used to determine the solution name from the source of the template exection.
  /// </summary>
  public const string Solution = "$setting:solution$";

  /// <summary>
  /// Used to determine the project from the source of the template execution.
  /// </summary>
  public const string ProjectName = "$setting:projectName$";

  /// <summary>
  /// Used to get the destination variable.
  /// </summary>
  public const string Destination = "$setting:destination$";

  /// <summary>
  /// Used to get the src variable.
  /// </summary>
  public const string Source = "$setting:src$";
}

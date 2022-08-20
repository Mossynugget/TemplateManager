namespace TemplateManagerModels.Models.Enums;

public record ReplacementSettingFileTypes
{
  /// <summary>
  /// String containing the search string for projects.
  /// </summary>
  public const string CsProject = "*.csproj";

  /// <summary>
  /// String containing the search string for solutions.
  /// </summary>
  public const string Solution = "*.sln";
}

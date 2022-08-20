using TemplateManagerModels.Models.Enums;

namespace TemplateManagerModels.Models.FileManager.FileReplacementHelpers;

internal static class CalculateProjectName
{

  /// <summary>
  /// A method used to replace the namespace variable in contents with the calculated namespace.
  /// </summary>
  /// <param name="destination">The destination of where the template fetch was initially called.</param>
  /// <returns></returns>
  public static string GetProjectName(string destination)
  {
    string projectFilePath;
    projectFilePath = FindFileTypeHelper.GetFilePath(destination, ReplacementSettingFileTypes.CsProject);

    return Path.GetFileNameWithoutExtension(projectFilePath);
  }
}

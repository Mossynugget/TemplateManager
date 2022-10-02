using TemplateManagerModels.Exceptions;
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

    try
    {
      projectFilePath = FindFileTypeHelper.GetFilePath(destination, ReplacementSettingFileTypes.CsProject);
    }
    catch
    {
      throw new InvalidSettingVariableException($"There was no project path identified originating from the path {destination}.");
    }

    return Path.GetFileNameWithoutExtension(projectFilePath);
  }
}

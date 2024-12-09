using TemplateManager.Models.Exceptions;
using TemplateManager.Models.Enums;

namespace TemplateManager.Models.FileManager.FileReplacementHelpers;

internal static class CalculateSolutionPathName
{
  /// <summary>
  /// A method used to replace the namespace variable in contents with the calculated namespace.
  /// </summary>
  /// <param name="destination">The destination of where the template fetch was initially called.</param>
  /// <returns>The sln path including the file name.</returns>
  public static string GetSolutionPath(string destination)
  {
    string solutionPathString;
    
    try
    {
      solutionPathString = FindFileTypeHelper.GetFilePath(destination, ReplacementSettingFileTypes.Solution);
    }
    catch
    {
      throw new InvalidSettingVariableException($"There was no solution path identified originating from the path {destination}.");
    }

    return solutionPathString;
  }
}

﻿using TemplateManagerModels.Models.Enums;

namespace TemplateManagerModels.Models.FileManager.FileReplacementHelpers;

internal static class CalculateSolutionPathName
{
  /// <summary>
  /// A method used to replace the namespace variable in contents with the calculated namespace.
  /// </summary>
  /// <param name="destination">The destination of where the template fetch was initially called.</param>
  /// <returns></returns>
  public static string GetSolutionPath(string destination)
  {
    string solutionPathString;
    solutionPathString = FindFileTypeHelper.GetFilePath(destination, ReplacementSettingFileTypes.Solution);
    var directoryName = Path.GetDirectoryName(solutionPathString);
    return directoryName;
  }
}
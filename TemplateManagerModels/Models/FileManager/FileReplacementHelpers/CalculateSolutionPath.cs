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
    do
    {
      destination = getParentPath(destination);
      solutionPathString = getSolutionString(destination);
    }
    while (string.IsNullOrEmpty(solutionPathString));

    return solutionPathString;
  }

  private static string getParentPath(string currentPath)
  {
    string? parentDirectory = Directory.GetParent(currentPath).FullName;
    if (parentDirectory is null)
    {
      throw new InvalidOperationException("The solution can't be calculated for the file because the destination isn't a part of a project.");
    }
    return parentDirectory;
  }

  private static string getSolutionString(string currentPath)
  {
    string[] presentSolution = Directory.GetFiles(currentPath, "*.sln", SearchOption.TopDirectoryOnly);
    
    if (presentSolution.Count() > 0) 
    {
      return currentPath;
    }

    return string.Empty;
  }
}

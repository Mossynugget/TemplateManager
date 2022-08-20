namespace TemplateManagerModels.Models.FileManager.FileReplacementHelpers;

internal static class FindFileTypeHelper
{
  /// <summary>
  /// A method used to replace the namespace variable in contents with the calculated namespace.
  /// </summary>
  /// <param name="destination">The destination of where the template fetch was initially called.</param>
  /// <returns></returns>
  public static string GetFilePath(string destination, string fileType)
  {
    string filePath;
    do
    {
      destination = getParentPath(destination);
      filePath = getSolutionString(destination, fileType);
    }
    while (string.IsNullOrEmpty(filePath));

    return filePath;
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

  private static string getSolutionString(string currentPath, string fileType)
  {
    string[] presentSolution = Directory.GetFiles(currentPath, fileType, SearchOption.TopDirectoryOnly);
    
    if (presentSolution.Count() > 0) 
    {
      return presentSolution[0];
    }

    return string.Empty;
  }
}

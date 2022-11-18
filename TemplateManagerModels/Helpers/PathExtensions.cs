using System.Runtime.InteropServices;

namespace TemplateManagerModels.Helpers;

public static class PathExtensions
{
  private static bool isLinux => RuntimeInformation.IsOSPlatform(OSPlatform.Linux);
  private static bool isOSX => RuntimeInformation.IsOSPlatform(OSPlatform.OSX);
  private static bool isWindows => RuntimeInformation.IsOSPlatform(OSPlatform.Windows);

  public static string GetRoot()
  {
    return Path.Combine(Path.GetPathRoot(Environment.SystemDirectory), "CodeTemplates");
  }

  public static string GetPath(this string path)
  {
    if (isLinux || isOSX)
    {
      return path.Replace("\\\\", "\\").Replace("\\", "/");
    }
    else
    {
      return path.Replace("\\\\", "\\").Replace("/", "\\");
    }
  }

  public static string FindDirectory(this string route, string folderName)
  {
    string filePath;
    do
    {
      filePath = getFolderPathString(route, folderName);
      route = getParentPath(route);
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

  private static string getFolderPathString(string currentPath, string folderName)
  {
    string[] presentSolution = Directory.GetDirectories(currentPath, folderName, SearchOption.TopDirectoryOnly);

    if (presentSolution.Count() > 0)
    {
      return presentSolution[0];
    }

    return string.Empty;
  }
}

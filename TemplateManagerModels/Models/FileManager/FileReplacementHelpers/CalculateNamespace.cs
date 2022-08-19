using TemplateManagerModels.Models.Enums;

namespace TemplateManagerModels.Models.FileManager.FileReplacementHelpers;

internal static class CalculateNamespace
{
  /// <summary>
  /// A method used to replace the namespace variable in contents with the calculated namespace.
  /// </summary>
  /// <param name="destination"></param>
  /// <param name="content"></param>
  /// <returns></returns>
  public static string ReplaceNamespace(this string content, string destination)
  {
    if (content.Contains(ReplacementSettingType.Namespace) == false)
    {
      return content;
    }

    string currentPath = destination;
    string namespaceString;
    do
    {
      currentPath = getParentPath(currentPath);
      namespaceString = getNamespaceString(currentPath, destination);
    }
    while (string.IsNullOrEmpty(namespaceString));

    content = content.Replace(ReplacementSettingType.Namespace, namespaceString);
    return content;
  }

  private static string getParentPath(string currentPath)
  {
    string? parentDirectory = Directory.GetParent(currentPath).FullName;
    if (parentDirectory is null)
    {
      throw new InvalidOperationException("The namespace can't be calculated for the file because the destination isn't a part of a project.");
    }
    return parentDirectory;
  }

  private static string getNamespaceString(string currentPath, string destination)
  {
    string namespaceString = string.Empty;
    string[] presentProjects = Directory.GetFiles(currentPath, "*.csproj", SearchOption.TopDirectoryOnly);
    
    if (presentProjects.Count() > 0) 
    {
      string projectAsNamespace = Path.GetFileNameWithoutExtension(presentProjects[0]);
      string pathAsNamespace = destination.Replace(currentPath, "").Replace("\\", ".").TrimEnd('.');
      namespaceString = $"{projectAsNamespace}{pathAsNamespace}";
    }

    return namespaceString;
  }
}

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

    var projectFilePath = FindFileTypeHelper.GetFilePath(destination, ReplacementSettingFileTypes.CsProject);
    var namespaceString = getNamespaceString(projectFilePath, destination);
    content = content.Replace(ReplacementSettingType.Namespace, namespaceString);

    return content;
  }

  private static string getNamespaceString(string projectFilePath, string destination)
  {
    string projectAsNamespace = Path.GetFileNameWithoutExtension(projectFilePath);
    string projectPath = Path.GetDirectoryName(projectFilePath);
    string pathAsNamespace = destination.Replace(projectPath, "").Replace("\\", ".").TrimEnd('.');
    return $"{projectAsNamespace}{pathAsNamespace}";
  }
}

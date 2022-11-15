using System.Runtime.InteropServices;

namespace TemplateManagerModels.Helpers;

public static class PathExtensions
{
  public static string GetRoot()
  {
    return Path.Combine(Path.GetPathRoot(Environment.SystemDirectory), "CodeTemplates");
  }

  private static bool isLinux()
  {
    return RuntimeInformation.IsOSPlatform(OSPlatform.Linux);
  }

  private static bool isWindows()
  {
    return RuntimeInformation.IsOSPlatform(OSPlatform.Windows);
  }
}

using System.Runtime.InteropServices;

namespace TemplateManagerModels.Helpers;

public static class OsHelper
{
    public static bool IsLinux => RuntimeInformation.IsOSPlatform(OSPlatform.Linux);
    public static bool IsOSX => RuntimeInformation.IsOSPlatform(OSPlatform.OSX);
    public static bool IsWindows => RuntimeInformation.IsOSPlatform(OSPlatform.Windows);
}

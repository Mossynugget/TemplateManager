using TemplateManager.Models.Exceptions;
using TemplateManager.Models.Enums;

namespace TemplateManager.Models.FileManager.FileReplacementHelpers;

internal class CalculateSrcPath
{
  /// <summary>
  /// A method used to get the path to the src folder navigating up from the selected destination.
  /// </summary>
  /// <param name="destination">The destination of where the template fetch was initially called.</param>
  /// <returns>The path to the src file.</returns>
  internal static string GetSrcPath(string destination)
  {
    try
    {
      if (destination.Contains(ReplacementSettingFileTypes.Source))
      {
        return destination.Substring(0, destination.IndexOf(ReplacementSettingFileTypes.Source) + 3);
      }
      string[] folders = Directory.GetDirectories(destination, ReplacementSettingFileTypes.Source, SearchOption.TopDirectoryOnly);
      return Path.Combine(destination, folders[0]);
    }
    catch
    {
      throw new InvalidSettingVariableException($"There was no source folder originating from the path {destination}.");
    }
  }
}

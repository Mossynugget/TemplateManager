namespace TemplateManager.Models.Enums;

public record TemplateFileTypes
{
  /// <summary>
  /// Used to specify that the template type is a file and will generate a new file.
  /// This is the default option.
  /// </summary>
  public const string File = "file";

  /// <summary>
  /// Used to specify that the template type is a snippet.
  /// Must co-exist with template "LineIdentifier", which will specify the lines to put the snippet under.
  /// </summary>
  public const string Snippet = "snippet";
}

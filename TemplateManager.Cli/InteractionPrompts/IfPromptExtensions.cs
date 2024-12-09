using Sharprompt;
using TemplateManager.Models.Helpers;
using TemplateManager.Models.Dtos;

namespace TemplateManager.Cli.InteractionPrompts;

public static class IfPromptsExtensions
{
  public static void ApplyIfs(this List<ReplacementVariableDto> replacementDictionary)
  {
    if (replacementDictionary.None())
      return;

    if (OsHelper.IsWindows || OsHelper.IsOSX)
    {
      replacementDictionary.SharpromptIfs();
    }
    else
    {
      replacementDictionary.CmdIfs();
    }
  }

  public static bool ApplyIf(string question)
  {
    if (OsHelper.IsWindows || OsHelper.IsOSX)
    {
      return SharpromptIf(question);
    }
    return CmdIf(question);
  }

  private static void SharpromptIfs(this List<ReplacementVariableDto> replacementDictionary)
  {
    var selectedProperties = Prompt.MultiSelect("Which of these would you like to apply?",
      replacementDictionary
      .Where(x => x.allowedType == TypeCode.Boolean)
      .Select(x => x.ReadableKey)
      .ToArray(),
      minimum: 0);

    foreach (var replacementIf in replacementDictionary.Where(x => selectedProperties.Contains(x.ReadableKey)))
    {
      replacementIf.SetValue(true);
    }
  }

  private static void CmdIfs(this List<ReplacementVariableDto> replacementDictionary)
  {
    foreach (var replacementIf in replacementDictionary)
    {
      Console.WriteLine($"Would you like to {replacementIf.ReadableKey}?\nPlease use 'y' for yes and 'n' for no.");
      var response = Console.ReadLine() ?? string.Empty;
      bool responseBoolValue = response.ToUpperInvariant()[0] == 'Y';
      replacementIf.SetValue(responseBoolValue);
    }
  }

  private static bool SharpromptIf(string question)
  {
    return Prompt.Confirm(question);
  }

  private static bool CmdIf(string question)
  {
    Console.WriteLine(question);
    var response = Console.ReadLine() ?? string.Empty;
    return response.ToUpperInvariant()[0] == 'Y';
  }
}

using Sharprompt;
using TemplateManagerModels.Helpers;
using TemplateManagerModels.Models.Dtos;
using TemplateManagerModels.Models.Helpers;

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
}

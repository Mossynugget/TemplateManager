using Sharprompt;
using System;
using TemplateManagerModels.Helpers;
using TemplateManagerModels.Models.Dtos;
using TemplateManagerModels.Models.Helpers;

namespace TemplateManager.Cli.InteractionPrompts;

public static class ValuePromptExtensions
{
  public static void ApplyValues(this List<ReplacementVariableDto> replacementDictionary)
  {
    if (replacementDictionary.None())
      return;

    foreach (var replacement in replacementDictionary)
    {
      if (OsHelper.IsWindows || OsHelper.IsOSX)
      {
        replacement.SharpromptInput();
      }
      else
      {
        replacement.CmdInput();
      }
    }
  }

  private static void SharpromptInput(this ReplacementVariableDto replacement)
  {
    replacement.SetValue(Prompt.Input<string>($"Please enter a value for {replacement.ReadableKey}"));
  }

  private static void CmdInput(this ReplacementVariableDto replacement)
  {
    Console.WriteLine($"Please enter a value for {replacement.ReadableKey}");

    var response = Console.ReadLine() ?? string.Empty;
    replacement.SetValue(response);
  }
}

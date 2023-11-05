using Sharprompt;
using System;
using TemplateManager.Models.Helpers;
using TemplateManager.Models.Dtos;
using TemplateManager.Models.Helpers;

namespace TemplateManager.Cli.InteractionPrompts;

public static class SelectPromptExtensions
{
  public static void ApplySelects(this List<ReplacementVariableDto> replacementDictionary)
  {
    if (replacementDictionary.None())
      return;

    foreach (var replacement in replacementDictionary)
    {
      if (OsHelper.IsWindows || OsHelper.IsOSX)
      {
        replacement.SharpromptSelect();
      }
      else
      {
        replacement.CmdSelect();
      }
    }
  }

  public static string ApplySelect(this string[] options, string message = "Select your template")
  {
    if (OsHelper.IsWindows || OsHelper.IsOSX)
    {
      return options.SharpromptSelect(message);
    }
    else
    {
      return options.CmdSelect(message);
    }
  }

  private static void SharpromptSelect(this ReplacementVariableDto replacement)
  {
    replacement.SetValue(Prompt.Select<string>($"Please select a {replacement.ReadableKey}", replacement.Options));
  }

  private static void CmdSelect(this ReplacementVariableDto replacement)
  {
    Console.WriteLine($"Please select a value for {replacement.Key.Replace("$", "").AddSpacesToSentence()}?\n from the following list by inserting to corresponding number.");

    for (int i = 1; i < replacement.Options!.Length + 1; i++)
    {
      Console.WriteLine($"{i}) {replacement.Options[i-1]}");
    }

    var response = int.Parse(Console.ReadLine() ?? string.Empty);
    var selected = replacement.Options[response-1];

    replacement.SetValue(selected);
  }

  private static string SharpromptSelect(this string[] options, string message)
  {
    return Prompt.Select<string>(message, options);
  }

  private static string CmdSelect(this string[] options, string message)
  {
    Console.WriteLine(message);

    for (int i = 1; i < options!.Length + 1; i++)
    {
      Console.WriteLine($"{i}) {options[i - 1]}");
    }

    var response = int.Parse(Console.ReadLine() ?? string.Empty);
    return options[response - 1];
  }
}

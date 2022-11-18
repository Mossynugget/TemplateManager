namespace TemplateManager.Cli;

using Sharprompt;
using System.Threading.Tasks;
using TemplateManager.Cli.TemplateNavigation;
using TemplateManagerModels.Helpers;
using TemplateManagerModels.Models;
using TemplateManagerModels.Models.Dtos;

class Startup
{
  static async Task Main(string[] args)
  {
    try
    {
      string selectedFile = await new TemplateNavigator().SelectTemplate().ConfigureAwait(false);

      TemplateFileGenerator templateFileGenerator = new(selectedFile);

      GetFileSettingsValues(templateFileGenerator);
      GetReplacementDictionaryValues(templateFileGenerator);
      await templateFileGenerator.GenerateFiles().ConfigureAwait(false);
      Console.WriteLine($"file(s) generated.");
    }
    // Used as an exit method.
    catch (Exception ex)
    {
      ExceptionHandler.ExceptionHandler.HandleException(ex);
    }

    static void GetFileSettingsValues(TemplateFileGenerator templateFileGenerator)
    {
      var fileSettingsDtoList = templateFileGenerator.GetFileTemplateSettings();

      foreach (var fileSettingsDto in fileSettingsDtoList)
      {
        fileSettingsDto.Destination = Environment.CurrentDirectory;
      }
      if (fileSettingsDtoList.Count == 1 && string.IsNullOrEmpty(fileSettingsDtoList[0].FileName))
      {
        fileSettingsDtoList[0].FileName = Prompt.Input<string>($"Please provide a file name for {fileSettingsDtoList[0].TemplateName}");
      }

      templateFileGenerator.MapFileTemplateSettings(fileSettingsDtoList);
    }

    static void GetReplacementDictionaryValues(TemplateFileGenerator templateFileGenerator)
    {
      var replacementDictionary = templateFileGenerator.GetReplacementDictionary();

      ApplyIfStatements(replacementDictionary);

      foreach (var replacement in replacementDictionary.Where(x => x.allowedType != TypeCode.Boolean))
      {
        if (replacement.Options != null)
        {
          replacement.SetValue(Prompt.Select<string>($"Please select a {replacement.ReadableKey}", replacement.Options));
        }
        else
        {
          replacement.SetValue(Prompt.Input<string>($"Please enter a value for {replacement.ReadableKey}"));
        }
      }

      templateFileGenerator.MapReplacementDictionary(replacementDictionary);
    }
  }

  private static void ApplyIfStatements(List<ReplacementVariableDto> replacementDictionary)
  {
    if (replacementDictionary.None(x => x.allowedType == TypeCode.Boolean)) return;

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
}
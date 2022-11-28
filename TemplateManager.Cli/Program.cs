namespace TemplateManager.Cli;

using Sharprompt;
using System.Threading.Tasks;
using TemplateManager.Cli.InteractionPrompts;
using TemplateManager.Cli.TemplateNavigation;
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

      replacementDictionary
        .Where(x => x.allowedType == TypeCode.Boolean)
        .ToList()
        .ApplyIfs();

      replacementDictionary
          .Where(x => x.allowedType != TypeCode.Boolean && x.Options != null)
          .ToList()
          .ApplySelects();

      replacementDictionary
          .Where(x => x.allowedType != TypeCode.Boolean && x.Options == null)
          .ToList()
          .ApplyValues();

      templateFileGenerator.MapReplacementDictionary(replacementDictionary);
    }
  }
}
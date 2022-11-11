namespace TemplateManager.Cli;

using Sharprompt;
using System.Threading.Tasks;
using TemplateManager.Cli.TemplateNavigation;
using TemplateManagerModels.Models;
using TemplateManagerModels.Models.Helpers;

class TestClass
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
      if (fileSettingsDtoList.Count == 1)
      {
        fileSettingsDtoList[0].FileName = Prompt.Input<string>($"Please provide a file name for {fileSettingsDtoList[0].TemplateName}");
      }

      templateFileGenerator.MapFileTemplateSettings(fileSettingsDtoList);
    }

    static void GetReplacementDictionaryValues(TemplateFileGenerator templateFileGenerator)
    {
      var replacementDictionary = templateFileGenerator.GetReplacementDictionary();

      foreach (var replacement in replacementDictionary)
      {
        if (replacement.Options != null)
        {
          replacement.SetValue(Prompt.Select<string>($"Please select a {replacement.Key.Replace("$", "").AddSpacesToSentence()}", replacement.Options));
        }
        else if (replacement.allowedType == TypeCode.Boolean)
        {
          replacement.SetValue(Prompt.Confirm($"Would you like to {replacement.Key.Replace("$", "").AddSpacesToSentence()}?", defaultValue: true));
        }
        else
        {
          replacement.SetValue(Prompt.Input<string>($"Please enter a value for {replacement.Key.Replace("$", "").AddSpacesToSentence()}"));
        }
      }

      templateFileGenerator.MapReplacementDictionary(replacementDictionary);
    }
  }
}
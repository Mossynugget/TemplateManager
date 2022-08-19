﻿namespace TemplateManager.Cli;

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
    catch (InvalidOperationException ex)
    {
      Console.WriteLine($"{ex.Message}.");
    }

    static void GetFileSettingsValues(TemplateFileGenerator templateFileGenerator)
    {
      var fileSettingsDtoList = templateFileGenerator.GetFileTemplateSettings();

      if (fileSettingsDtoList.Count == 1)
      {
        // fileSettingsDtoList[0].Destination = Environment.CurrentDirectory;
      }

      foreach (var fileSettingsDto in fileSettingsDtoList)
      {
        Console.WriteLine($"Please provide a file name for {fileSettingsDto.TemplateName}.");
        fileSettingsDto.FileName = Console.ReadLine();
      }

      templateFileGenerator.MapFileTemplateSettings(fileSettingsDtoList);
    }

    static void GetReplacementDictionaryValues(TemplateFileGenerator templateFileGenerator)
    {
      var replacementDictionary = templateFileGenerator.GetReplacementDictionary();

      foreach (var replacement in replacementDictionary)
      {
        if (replacement.allowedType == TypeCode.Boolean)
        {
          Console.WriteLine($"Would you like to {replacement.Key.Replace("$", "").AddSpacesToSentence()}?\nPlease use 'y' for yes and 'n' for no.");
        }
        else
        {
          Console.WriteLine($"Please enter a value for {replacement.Key.Replace("$", "").AddSpacesToSentence()}");
        }
        replacement.SetValue(Console.ReadLine() ?? string.Empty);
      }

      templateFileGenerator.MapReplacementDictionary(replacementDictionary);
    }
  }
}
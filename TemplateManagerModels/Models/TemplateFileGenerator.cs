using TemplateManagerModels.Models.Dtos;
using TemplateManagerModels.Models.FileManager;

namespace TemplateManagerModels.Models;

public class TemplateFileGenerator
{
  private FileTemplateDictionary FileTemplateDictionary;

  public TemplateFileGenerator(string fileTemplate)
  {
    FileTemplateDictionary = new FileTemplateDictionary();
    this.FileTemplateDictionary.AddTemplateFile(fileTemplate);
  }

  public async Task GenerateFiles()
  {
    await this.FileTemplateDictionary.GenerateFiles().ConfigureAwait(false);
  }

  public List<FileSettingsDto> GetFileTemplateSettings()
  {
    return this.FileTemplateDictionary.GetFileSettingDtoList();
  }

  public void MapFileTemplateSettings(List<FileSettingsDto> fileSettings)
  {
    this.FileTemplateDictionary.MapFileTemplateSettings(fileSettings);
  }

  public Dictionary<string, string?> GetReplacementDictionary()
  {
    return this.FileTemplateDictionary.ReplacementDictionary.GetReplacementVariablesAsDictionary();
  }

  public void MapReplacementDictionary(Dictionary<string, string?> replacementDictionary)
  {
    this.FileTemplateDictionary.ReplacementDictionary.MapDictionaryToReplacementVariables(replacementDictionary);
  }
}

using TemplateManager.Models.Dtos;
using TemplateManager.Models.FileManager;

namespace TemplateManager.Models;

public class TemplateFileGenerator
{
  private FileTemplateDictionary FileTemplateDictionary;

  public  TemplateFileGenerator(string fileTemplate)
  {
    this.FileTemplateDictionary = new FileTemplateDictionary();
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

  public List<ReplacementVariableDto> GetReplacementDictionary()
  {
    return this.FileTemplateDictionary.ReplacementDictionary.GetReplacementVariablesAsDictionary();
  }

  public void MapReplacementDictionary(List<ReplacementVariableDto> replacementDictionary)
  {
    this.FileTemplateDictionary.ReplacementDictionary.MapDictionaryToReplacementVariables(replacementDictionary);
  }
}

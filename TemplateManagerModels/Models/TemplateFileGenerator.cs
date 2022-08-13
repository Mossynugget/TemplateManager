using TemplateManagerModels.Models.FileManager;

namespace TemplateManagerModels.Models;

public class TemplateFileGenerator
{
  private FileTemplateDictionary FileTemplateDictionary;

  public TemplateFileGenerator(string[] fileToGenerateList)
  {
    this.FileTemplateDictionary = new (fileToGenerateList);
  }

  public TemplateFileGenerator(string fileToGenerateList, string fileName)
  {
    this.FileTemplateDictionary = new(fileToGenerateList, fileName);
  }
  public async Task GenerateFiles()
  {
    await this.FileTemplateDictionary.GenerateFiles().ConfigureAwait(false);
  }

  public Dictionary<string, string?> GetReplacementDictionary()
  {
    return this.FileTemplateDictionary.ReplacementDictionary.GetReplacementVariablesAsDictionary();
  }

  public void SaveReplacementDictionary(Dictionary<string, string?> replacementDictionary)
  {
    this.FileTemplateDictionary.ReplacementDictionary.MapDictionaryToReplacementVariables(replacementDictionary);
  }
}

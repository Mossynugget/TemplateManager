using TemplateManagerModels.Models.FileManager;

namespace TemplateManagerModels.Models;

public class TemplateFileGenerator
{
  private FileTemplateDictionary FileTemplateDictionary;

  public TemplateFileGenerator(string[] fileToGenerateList)
  {
    this.FileTemplateDictionary = new (fileToGenerateList);
  }
}

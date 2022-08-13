namespace TemplateManagerModels.Models.FileManager
{
  internal class FileTemplateDictionary
  {
    internal ReplacementDictionary ReplacementDictionary { get; set; }
    internal List<FileTemplate> FileTemplateList { get; set; }

    internal FileTemplateDictionary(string[] templateNameList)
    {
      this.FileTemplateList = new();
      this.ReplacementDictionary = new();

      foreach (var templateName in templateNameList)
      {
        this.FileTemplateList.Add(new FileTemplate(templateName));
      }
      foreach (var fileTemplate in this.FileTemplateList)
      {
        this.ReplacementDictionary.CreateReplacementVariableList(fileTemplate.Contents);
      }
    }
  }
}

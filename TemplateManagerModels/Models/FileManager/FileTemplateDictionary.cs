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

    internal FileTemplateDictionary(string templateName, string fileName)
    {
      this.FileTemplateList = new();
      this.FileTemplateList.Add(new FileTemplate(templateName, fileName));
      this.ReplacementDictionary = new(this.FileTemplateList[0].Contents);
    }

    internal async Task GenerateFiles()
    {
      foreach (var fileTemplate in this.FileTemplateList)
      {
        fileTemplate.ApplyReplacementVariableDictionary(this.ReplacementDictionary);
        await fileTemplate.GenerateFileAsync().ConfigureAwait(false);
      }
    }
  }
}

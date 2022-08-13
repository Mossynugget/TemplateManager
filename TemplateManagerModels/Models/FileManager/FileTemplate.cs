using Microsoft.VisualBasic;

namespace TemplateManagerModels.Models.FileManager;

internal class FileTemplate
{
  internal string TemplateFilePath;
  internal string Name;
  internal string FileExtension;
  internal string Contents;
  internal string Destination = "D:/Clients/ItemTemplateMauiApp/ItemTemplateCreatorApi/TestFileGenerationLocation/";
  private string _finalPath {
    get 
    {
      var finalPath = Path.Combine(Destination, Name);
      finalPath = Path.ChangeExtension(finalPath, FileExtension);
      return finalPath;
    } 
  }

  internal FileTemplate(string templateFilePath, string fileName = "")
  {
    TemplateFilePath = templateFilePath;
    Name = fileName;
    FileExtension = Path.GetExtension(templateFilePath);
    Contents = File.ReadAllText(templateFilePath);
  }

  internal void SetDestination(string destination)
  {
    this.Destination = destination;
  }

  internal void ApplyReplacementVariableDictionary(ReplacementDictionary replacementDictionary)
  {
    Contents = Contents.Replace(Path.GetFileNameWithoutExtension(TemplateFilePath), Name);

    foreach (var replacementVariable in replacementDictionary.ReplacementVariableList)
    {
      Contents = Contents.Replace(replacementVariable.Key, replacementVariable.Value);
    }
  }

  internal async Task GenerateFileAsync()
  {
    await File.WriteAllTextAsync(_finalPath, Contents).ConfigureAwait(false);
  }
}

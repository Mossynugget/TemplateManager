using TemplateManagerModels.Models.Dtos;

namespace TemplateManagerModels.Models.FileManager;

internal class FileTemplate
{
  internal readonly string TemplateFilePath;
  internal readonly string FileExtension;
  internal string? FileName;
  internal string Contents;
  internal string Destination = "D:/Clients/ItemTemplateMauiApp/ItemTemplateCreatorApi/TestFileGenerationLocation/";
  private string _finalPath {
    get 
    {
      var finalPath = Path.Combine(Destination, FileName);
      finalPath = Path.ChangeExtension(finalPath, FileExtension);
      return finalPath;
    } 
  }

  internal FileTemplate(string templateFilePath)
  {
    TemplateFilePath = templateFilePath;
    FileExtension = Path.GetExtension(templateFilePath);
    Contents = File.ReadAllText(templateFilePath);
  }

  internal void SetFileSettings(FileSettingsDto fileSettings)
  {
    this.Destination = fileSettings.Destination;
    this.FileName = fileSettings.FileName;
  }

  internal void ApplyReplacementVariableDictionary(ReplacementDictionary replacementDictionary)
  {
    Contents = Contents.Replace(Path.GetFileNameWithoutExtension(TemplateFilePath), FileName);

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

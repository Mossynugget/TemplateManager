using TemplateManagerModels.Models.Dtos;
using TemplateManagerModels.Models.FileManager.FileReplacementHelpers;
using TemplateManagerModels.Models.FileManager.VariableReplacement;

namespace TemplateManagerModels.Models.FileManager;

internal class FileTemplate
{
  internal readonly string TemplateFilePath;
  internal readonly string FileExtension;
  internal string? FileName;
  internal string Contents;
  internal string Destination = "D:\\Clients\\TemplatingTests\\subfolder\\";
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
    Contents = replacementDictionary.ReplaceContents(Contents);

    this.loadAdditionalReplacements();
  }

  internal async Task GenerateFileAsync()
  {
    await File.WriteAllTextAsync(_finalPath, Contents).ConfigureAwait(false);
  }

  private void loadAdditionalReplacements()
  {
    Contents = Contents.Replace(Path.GetFileNameWithoutExtension(TemplateFilePath), FileName);
    Contents = Contents.ReplaceNamespace(this.Destination);
  }
}

using System.IO;
using TemplateManagerModels.Models.Dtos;
using TemplateManagerModels.Models.Enums;
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
  internal string? CalulatedDestination = null;
  private string _finalPath {
    get 
    {
      var finalPath = Path.Combine(CalulatedDestination ?? Destination, FileName);
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
    this.FileName = fileSettings.FileName ?? this.FileName;
  }

  internal void ApplyReplacementVariableDictionary(ReplacementDictionary replacementDictionary)
  {
    Contents = replacementDictionary.ReplaceContents(Contents);
    FileName = replacementDictionary.ReplaceContents(FileName);
    CalulatedDestination = replacementDictionary.ReplaceContents(CalulatedDestination ?? string.Empty);
    this.loadAdditionalReplacements();

    CalulatedDestination = CalulatedDestination != null ? replacementDictionary.ReplaceContents(CalulatedDestination) : Destination;
  }

  internal async Task GenerateFileAsync()
  {
    validateFolderExists(_finalPath);
    await File.WriteAllTextAsync(_finalPath, Contents).ConfigureAwait(false);
  }

  private void loadAdditionalReplacements()
  {
    Contents = Contents.Replace(Path.GetFileNameWithoutExtension(TemplateFilePath), FileName);
    LoadModuleSetting();
    Contents = Contents.ReplaceNamespace(this.Destination);
  }

  private void LoadModuleSetting()
  {
    if (this.Contents.Contains(ReplacementSettingType.Solution)
      || this.FileName!.Contains(ReplacementSettingType.Solution)
      || (this.CalulatedDestination?.Contains(ReplacementSettingType.Solution) ?? false))
    {
      var solutionPathString = CalculateSolutionPathName.GetSolutionPath(this.Destination);

      Contents = Contents.Replace(ReplacementSettingType.SolutionPath, solutionPathString);
      FileName = FileName?.Replace(ReplacementSettingType.SolutionPath, solutionPathString);
      CalulatedDestination = CalulatedDestination?.Replace(ReplacementSettingType.SolutionPath, solutionPathString);

      var solutionString = Path.GetFileName(solutionPathString);
      Contents = Contents.Replace(ReplacementSettingType.Solution, solutionString);
      FileName = FileName?.Replace(ReplacementSettingType.Solution, solutionString);
      CalulatedDestination = CalulatedDestination?.Replace(ReplacementSettingType.Solution, solutionString);
    }
  }

  private void validateFolderExists(string destination)
  {
    if (Directory.Exists(destination))
    {
      return;
    }

    Directory.CreateDirectory(Path.GetDirectoryName(destination));
  }
}

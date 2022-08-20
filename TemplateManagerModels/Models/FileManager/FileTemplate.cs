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
    FileName = replacementDictionary.ReplaceValueContents(FileName);
    CalulatedDestination = replacementDictionary.ReplaceValueContents(CalulatedDestination ?? string.Empty);
    this.loadAdditionalReplacements(replacementDictionary);
  }

  internal async Task GenerateFileAsync()
  {
    await File.WriteAllTextAsync(_finalPath, Contents).ConfigureAwait(false);
  }

  private void loadAdditionalReplacements(ReplacementDictionary replacementDictionary)
  {
    Contents = Contents.Replace(Path.GetFileNameWithoutExtension(TemplateFilePath), FileName);
    LoadSolutionPathVariables();
    CalulatedDestination = CalulatedDestination != null ? replacementDictionary.ReplaceValueContents(CalulatedDestination) : Destination;
    validateFolderExists(_finalPath);
    LoadProjectNameVariables();
    Contents = Contents.ReplaceNamespace(CalulatedDestination);
  }

  private void LoadSolutionPathVariables()
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

  private void LoadProjectNameVariables()
  {
    if (this.Contents.Contains(ReplacementSettingType.ProjectName)
      || this.FileName!.Contains(ReplacementSettingType.ProjectName)
      || (this.CalulatedDestination?.Contains(ReplacementSettingType.ProjectName) ?? false))
    {
      var projectName = CalculateProjectName.GetProjectName(this.Destination);

      Contents = Contents.Replace(ReplacementSettingType.ProjectName, projectName);
      FileName = FileName?.Replace(ReplacementSettingType.ProjectName, projectName);
      CalulatedDestination = CalulatedDestination?.Replace(ReplacementSettingType.ProjectName, projectName);
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

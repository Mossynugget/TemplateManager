using TemplateManagerModels.Models.Dtos;
using TemplateManagerModels.Models.Enums;
using TemplateManagerModels.Models.FileManager.FileReplacementHelpers;
using TemplateManagerModels.Models.FileManager.VariableReplacement;

namespace TemplateManagerModels.Models.FileManager.Templates;

internal class FileTemplate
{
  internal readonly string TemplateFilePath;
  internal readonly string FileExtension;
  internal string? FileName;
  internal string Contents;
  internal string Destination = "D:\\Clients\\TemplatingTests\\subfolder\\";
  internal string? CalulatedDestination = null;
  private string _finalPath
  {
    get
    {
      var finalPath = Path.Combine(CalulatedDestination ?? Destination, FileName ?? string.Empty);
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
    Destination = fileSettings.Destination;
    FileName = fileSettings.FileName ?? FileName;
  }

  internal void ApplyReplacementVariableDictionary(ReplacementDictionary replacementDictionary)
  {
    Contents = replacementDictionary.ApplyAllReplaceLists(Contents);
    FileName = replacementDictionary.ReplaceValueContents(FileName);

    loadAdditionalReplacements(replacementDictionary);
  }

  internal async Task GenerateFileAsync()
  {
    await File.WriteAllTextAsync(_finalPath, Contents).ConfigureAwait(false);
  }

  private void loadAdditionalReplacements(ReplacementDictionary replacementDictionary)
  {
    Contents = Contents.Replace(Path.GetFileNameWithoutExtension(TemplateFilePath), FileName);
    LoadSolutionPathVariables();
    calculateDestination(replacementDictionary);
    validateFolderExists(_finalPath);
    LoadProjectNameVariables();
    Contents = Contents.ReplaceNamespace(CalulatedDestination);
  }

  private void calculateDestination(ReplacementDictionary replacementDictionary)
  {
    CalulatedDestination = replacementDictionary.ApplyAllReplaceLists(CalulatedDestination ?? string.Empty);

    if (CalulatedDestination.Contains(ReplacementSettingType.Destination))
    {
      CalulatedDestination = CalulatedDestination.Replace(ReplacementSettingType.Destination, string.Empty);
      CalulatedDestination = $"{Destination}{CalulatedDestination}";
    }

    CalulatedDestination = CalulatedDestination != null ? replacementDictionary.ReplaceValueContents(CalulatedDestination) : Destination;
    CalulatedDestination = CalulatedDestination.Replace("\\\\", "\\");
  }

  private void LoadSolutionPathVariables()
  {
    if (Contents.Contains(ReplacementSettingType.Solution)
      || FileName!.Contains(ReplacementSettingType.Solution)
      || (CalulatedDestination?.Contains(ReplacementSettingType.Solution) ?? false))
    {
      var solutionPathString = CalculateSolutionPathName.GetSolutionPath(Destination);

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
    if (Contents.Contains(ReplacementSettingType.ProjectName)
      || FileName!.Contains(ReplacementSettingType.ProjectName)
      || (CalulatedDestination?.Contains(ReplacementSettingType.ProjectName) ?? false))
    {
      var projectName = CalculateProjectName.GetProjectName(Destination);

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

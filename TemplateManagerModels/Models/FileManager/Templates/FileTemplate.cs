using System.Text;
using TemplateManagerModels.Helpers;
using TemplateManagerModels.Models.Dtos;
using TemplateManagerModels.Models.Enums;
using TemplateManagerModels.Models.FileManager.FileReplacementHelpers;
using TemplateManagerModels.Models.FileManager.VariableReplacement;
using TemplateManagerModels.Models.Helpers;

namespace TemplateManagerModels.Models.FileManager.Templates;

internal class FileTemplate
{
  internal readonly string TemplateFilePath;
  internal readonly string FileExtension;
  internal string? FileName;
  internal string? LineIdentifier;
  internal string Contents;
  internal string? FileType;
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
    Destination = fileSettings.Destination.GetPath();
    FileName = fileSettings.FileName ?? FileName;
  }

  internal void ApplyReplacementVariableDictionary(ReplacementDictionary replacementDictionary)
  {
    FileName ??= string.Empty;
    Contents = replacementDictionary.ApplyAllReplaceLists(Contents);
    FileName = replacementDictionary.ApplyAllReplaceLists(FileName);

    loadAdditionalReplacements(replacementDictionary);
  }

  internal async Task GenerateFileAsync()
  {
    if (FileType?.ToLower() == TemplateFileTypes.Snippet)
    {
      await applySnippet().ConfigureAwait(false);

      return;
    }

    await File.WriteAllTextAsync(_finalPath.GetPath(), Contents, Encoding.UTF8).ConfigureAwait(false);
  }

  private async Task applySnippet()
  {
    StreamReader reader = new StreamReader(_finalPath.GetPath());
    var fileContents = reader.ReadToEnd();
    reader.Dispose();

    var contents = $"{LineIdentifier!}{Environment.NewLine}{Contents}";
    fileContents = fileContents.Replace(LineIdentifier!, contents);

    await File.WriteAllTextAsync(_finalPath.GetPath(), fileContents, Encoding.UTF8).ConfigureAwait(false);
  }

  private void loadAdditionalReplacements(ReplacementDictionary replacementDictionary)
  {
    if (string.IsNullOrEmpty(Path.GetFileNameWithoutExtension(TemplateFilePath)) == false)
    {
      Contents = Contents.Replace(Path.GetFileNameWithoutExtension(TemplateFilePath), FileName);
    }
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

    if (CalulatedDestination.Contains(ReplacementSettingType.Source))
    {
      string sourcePath = CalculateSrcPath.GetSrcPath(Destination);
      CalulatedDestination = CalulatedDestination.Replace(ReplacementSettingType.Source, sourcePath);
    }

    CalulatedDestination = string.IsNullOrEmpty(CalulatedDestination) == false
      ? replacementDictionary.ReplaceValueContents(CalulatedDestination)
      : Destination;

    CalulatedDestination = CalulatedDestination.GetPath();
  }

  private void LoadSolutionPathVariables()
  {
    if (ListExtensions.ContainsAny(new List<string?> { Contents, FileName, CalulatedDestination },
      ReplacementSettingType.Solution
      , ReplacementSettingType.SolutionPath
      , ReplacementSettingType.Solution.ValueLowercase()))
    {
      var solutionPathString = CalculateSolutionPathName.GetSolutionPath(Destination);
      var directoryPath = Path.GetDirectoryName(solutionPathString);

      Contents = Contents.Replace(ReplacementSettingType.SolutionPath, directoryPath);
      FileName = FileName?.Replace(ReplacementSettingType.SolutionPath, directoryPath);
      CalulatedDestination = CalulatedDestination?.Replace(ReplacementSettingType.SolutionPath, directoryPath);

      var solutionString = Path.GetFileNameWithoutExtension(solutionPathString);

      Contents = Contents.Replace(ReplacementSettingType.Solution, solutionString);
      Contents = Contents.Replace(ReplacementSettingType.Solution.KeyLowercase(), solutionString.ValueLowercase());
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

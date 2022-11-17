using Newtonsoft.Json;
using TemplateManagerModels.Helpers;
using TemplateManagerModels.Models.Dtos;
using TemplateManagerModels.Models.Dtos.ImportGroup;
using TemplateManagerModels.Models.FileManager.Templates;
using TemplateManagerModels.Models.FileManager.VariableReplacement;

namespace TemplateManagerModels.Models.FileManager;

internal class FileTemplateDictionary
{
  internal ReplacementDictionary ReplacementDictionary { get; set; }
  internal List<FileTemplate> FileTemplateList { get; set; }

  internal FileTemplateDictionary()
  {
    this.FileTemplateList = new();
    this.ReplacementDictionary = new();
  }

  internal void AddTemplateFile(string templateName)
  {
    if (Path.GetExtension(templateName) == ".tmplt")
    {
      using (StreamReader r = new StreamReader(templateName))
      {
        string json = r.ReadToEnd();
        FileGroupDto fileGroup = JsonConvert.DeserializeObject<FileGroupDto>(json) ?? new();
        this.ReplacementDictionary.AddReplacementValues(fileGroup.ReplacementValues);
        this.addFileTemplates(fileGroup.Files, Path.GetDirectoryName(templateName));
      }
    }
    else
    {
      this.addFileTemplate(templateName);
    }
  }

  internal List<FileSettingsDto> GetFileSettingDtoList()
  {
    var fileSettingsDtoList = new List<FileSettingsDto>();
    foreach (var fileTemplate in this.FileTemplateList)
    {
      fileSettingsDtoList.Add(new FileSettingsDto(fileTemplate));
    }

    return fileSettingsDtoList;
  }

  internal void MapFileTemplateSettings(List<FileSettingsDto> fileSettingsDtoList)
  {
    foreach (var fileSettingDto in fileSettingsDtoList)
    {
      this.FileTemplateList
        .First(x => x.TemplateFilePath == fileSettingDto.Template)
        .SetFileSettings(fileSettingDto);
    }
  }

  private void addFileTemplate(string templateName)
  {
    var fileTemplate = new FileTemplate(templateName);
    this.FileTemplateList.Add(fileTemplate);
    this.ReplacementDictionary.CreateReplacementLists(fileTemplate.Contents);
  }

  private void addFileTemplates(List<FileGroupFileDto> fileGroupDtos, string templateGroupDirectory)
  {
    foreach (var fileGroupDto in fileGroupDtos)
    {
      string fullFilePath = Path.Combine(templateGroupDirectory.GetPath(), fileGroupDto.TemplateName.GetPath());
      var fileTemplate = new FileTemplate(fullFilePath);
      fileTemplate.CalulatedDestination = fileGroupDto.Destination.GetPath();
      fileTemplate.FileName = fileGroupDto.FileName;
      this.FileTemplateList.Add(fileTemplate);
      this.ReplacementDictionary.CreateReplacementLists(fileTemplate.Contents);
      this.ReplacementDictionary.CreateReplacementLists(fileTemplate.FileName);
      this.ReplacementDictionary.CreateReplacementLists(fileTemplate.CalulatedDestination);
    }
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

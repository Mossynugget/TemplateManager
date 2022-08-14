using TemplateManagerModels.Models.Dtos;

namespace TemplateManagerModels.Models.FileManager
{
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
      this.addFileTemplate(templateName);
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
      this.ReplacementDictionary.CreateReplacementVariableList(fileTemplate.Contents);
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

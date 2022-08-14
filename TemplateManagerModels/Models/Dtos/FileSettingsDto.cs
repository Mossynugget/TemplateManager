using TemplateManagerModels.Models.FileManager;

namespace TemplateManagerModels.Models.Dtos
{
  public class FileSettingsDto
  {
    public readonly string Template;
    public string TemplateName => Path.GetFileName(Template);
    public string Destination { get; set; }
    public string? FileName { get; set; }

    internal FileSettingsDto(FileTemplate template)
    {
      this.Template = template.TemplateFilePath;
      this.Destination = template.Destination;
    }
  }
}

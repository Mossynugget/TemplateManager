using TemplateManager.Models.FileManager.VariableReplacement;

namespace TemplateManager.Models.Dtos.ImportGroup;

public class FileGroupDto
{
  public List<ReplacementValue>? ReplacementValues { get; set; }

  public List<FileGroupFileDto>? Files { get; set; }
}

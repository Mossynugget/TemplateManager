using TemplateManagerModels.Models.FileManager.VariableReplacement;

namespace TemplateManagerModels.Models.Dtos.ImportGroup;

public class FileGroupDto
{
  public List<ReplacementValue>? ReplacementValues { get; set; }

  public List<FileGroupFileDto>? Files { get; set; }
}

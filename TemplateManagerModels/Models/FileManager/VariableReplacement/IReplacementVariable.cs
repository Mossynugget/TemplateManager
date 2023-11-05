using TemplateManager.Models.Enums;

namespace TemplateManager.Models.FileManager.VariableReplacement;

public interface IReplacementVariable
{
  public string Key { get; set; }
  public bool RequiresInput { get; set; }
  public ReplacementVariableType ReplacementVariableTypeEnum { get; }
  public TypeCode TypeCode { get; }

  internal abstract void SetValue(dynamic value);

  internal abstract string ApplyReplacementVariable(string contents);
}
using TemplateManager.Models.Enums;

namespace TemplateManager.Models.FileManager.VariableReplacement;

public abstract class ReplacementVariableAbstract : IReplacementVariable
{
  public string Key { get; set; }
  public bool RequiresInput { get; set; }
  public ReplacementVariableType ReplacementVariableTypeEnum { get; }
  public TypeCode TypeCode { get; }

  public ReplacementVariableAbstract(string Key, ReplacementVariableType ReplacementVariableTypeEnum, TypeCode TypeCode, bool requiresInput)
  {
    this.Key = Key;
    this.ReplacementVariableTypeEnum = ReplacementVariableTypeEnum;
    this.TypeCode = TypeCode;
    this.RequiresInput = requiresInput;
  }

  public abstract void SetValue(dynamic value);

  public abstract string ApplyReplacementVariable(string contents);
}

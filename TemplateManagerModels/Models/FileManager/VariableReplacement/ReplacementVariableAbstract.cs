using TemplateManagerModels.Models.Enums;

namespace TemplateManagerModels.Models.FileManager.VariableReplacement;

internal abstract class ReplacementVariableAbstract
{
  public string Key { get; set; }
  public ReplacementVariableType ReplacementVariableTypeEnum { get; }
  public TypeCode TypeCode { get; }

  internal ReplacementVariableAbstract(string Key, ReplacementVariableType ReplacementVariableTypeEnum, TypeCode TypeCode)
  {
    this.Key = Key;
    this.ReplacementVariableTypeEnum = ReplacementVariableTypeEnum;
    this.TypeCode = TypeCode;
  }

  internal abstract void SetValue(dynamic value);
}

namespace TemplateManagerModels.Models.FileManager.VariableReplacement;

internal class ReplacementValue : ReplacementVariableAbstract
{
  internal string? Value { get; private set; }

  public ReplacementValue(string key) :
    base(key, Enums.ReplacementVariableTypeEnum.VariableType, TypeCode.String)
  {
  }

  internal void SetValue(string value)
  {
    Value = value;
  }

  internal override void SetValue(dynamic value)
  {
    Value = value;
  }
}

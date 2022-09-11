using TemplateManagerModels.Models.Enums;

namespace TemplateManagerModels.Models.FileManager.VariableReplacement;

internal class ReplacementIf : ReplacementVariableAbstract
{
  internal bool Value { get; private set; } = false;
  internal string IfString => $"$if:{Key}$";
  internal string ElseString => $"$endif:{Key}$";
  internal int IfLength => IfString.Length;
  internal int EndifLength => ElseString.Length;

  public ReplacementIf(string key) :
    base(key, ReplacementVariableType.If, TypeCode.Boolean)
  {
  }

  internal override void SetValue(dynamic value)
  {
    Value = value;
  }
}

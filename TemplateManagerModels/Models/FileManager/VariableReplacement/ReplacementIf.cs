using TemplateManagerModels.Models.Enums;

namespace TemplateManagerModels.Models.FileManager.VariableReplacement;

internal class ReplacementIf : ReplacementVariableAbstract
{
  internal bool Value { get; private set; } = false;
  internal int IndexStart { get; private set; }
  internal int IndexEnd { get; private set; }
  internal string IfString => $"$if:{Key}$";
  internal string ElseString => $"$endif:{Key}$";
  internal int IfLength => IfString.Length;
  internal int EndifLength => ElseString.Length;

  public ReplacementIf(string key, int indexStart, int indexEnd) :
    base(key, ReplacementVariableType.If, TypeCode.Boolean)
  {
    IndexStart = indexStart;
    IndexEnd = indexEnd;
  }

  internal override void SetValue(dynamic value)
  {
    Value = value;
  }
}

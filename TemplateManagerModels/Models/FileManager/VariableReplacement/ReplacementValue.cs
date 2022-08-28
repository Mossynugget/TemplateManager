using Newtonsoft.Json;
using TemplateManagerModels.Models.Helpers;

namespace TemplateManagerModels.Models.FileManager.VariableReplacement;

public class ReplacementValue : ReplacementVariableAbstract
{
  internal string? Value { get; private set; }
  internal string? KeyComment => $"{this.Key.TrimEnd('$')}:comment$";
  internal string? ValueComment => Value?.AddSpacesToSentence() ?? string.Empty;

  public ReplacementValue(string key) :
    base(key, Enums.ReplacementVariableType.Value, TypeCode.String)
  {
  }

  [JsonConstructor]
  public ReplacementValue(string key, string value) :
    base(key, Enums.ReplacementVariableType.Value, TypeCode.String)
  {
    Value = value;
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

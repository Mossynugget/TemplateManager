using TemplateManagerModels.Models.Enums;
using TemplateManagerModels.Models.FileManager.VariableReplacement;
using TemplateManagerModels.Models.Helpers;

namespace TemplateManagerModels.Models.Dtos;

public class ReplacementVariableDto
{
  public readonly string Key;
  public string ReadableKey => Key.Replace("$", "").AddSpacesToSentence();
  public dynamic? Value { get; private set; }
  public string[]? Options { get; private set; }
  public TypeCode allowedType { get; private set; }

  internal readonly ReplacementVariableType ReplacementDictionaryType;

  internal ReplacementVariableDto(ReplacementVariableAbstract replacementDictionaryVariable, string[]? options = null)
  {
    this.Key = replacementDictionaryVariable.Key;
    this.Options = options;
    this.allowedType = replacementDictionaryVariable.TypeCode;
    this.ReplacementDictionaryType = replacementDictionaryVariable.ReplacementVariableTypeEnum;
  }

  public void SetValue(dynamic value)
  {
    if (this.allowedType == TypeCode.Boolean && value.GetType().Name == TypeCode.String.ToString())
    {
      this.Value = value || "y".Equals(value);
    }
    else if (value.GetType().Name != this.allowedType.ToString())
    {
      throw new InvalidOperationException($"The type passed into the Replacement DTO was invalid, expected {this.allowedType} but found {value.GetType().Name}");
    }
    else
    {
      this.Value = value;
    }
  }
}

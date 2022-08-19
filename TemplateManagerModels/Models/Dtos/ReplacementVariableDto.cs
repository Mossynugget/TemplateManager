using TemplateManagerModels.Models.Enums;
using TemplateManagerModels.Models.FileManager.VariableReplacement;

namespace TemplateManagerModels.Models.Dtos
{
  public class ReplacementVariableDto
  {
    public readonly string Key;
    internal dynamic? Value { get; private set; }
    public TypeCode allowedType { get; private set; }

    internal readonly ReplacementVariableType ReplacementDictionaryType;

    internal ReplacementVariableDto(ReplacementVariableAbstract replacementDictionaryVariable)
    {
      this.Key = replacementDictionaryVariable.Key;
      this.allowedType = replacementDictionaryVariable.TypeCode;
      this.ReplacementDictionaryType = replacementDictionaryVariable.ReplacementVariableTypeEnum;
    }

    public void SetValue(dynamic value)
    {
      if (this.allowedType == TypeCode.Boolean && value.GetType().Name == TypeCode.String.ToString())
      {
        this.Value = "y".Equals(value);
      }
      else if (value.GetType().Name != this.allowedType.ToString())
      {
        throw new InvalidOperationException("The type passed into the Replacement DTO was invalid.");
      }
      else
      {
        this.Value = value;
      }
    }
  }
}

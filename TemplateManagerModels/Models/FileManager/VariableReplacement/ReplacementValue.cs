using Microsoft.VisualBasic;
using Newtonsoft.Json;
using System.Text.RegularExpressions;
using TemplateManagerModels.Models.Helpers;

namespace TemplateManagerModels.Models.FileManager.VariableReplacement;

public class ReplacementValue : ReplacementVariableAbstract
{
  internal string? Value { get; private set; }

  public ReplacementValue(string key) :
    base(key, Enums.ReplacementVariableType.Value, TypeCode.String, true)
  {
  }

  [JsonConstructor]
  public ReplacementValue(string key, string value) :
    base(key, Enums.ReplacementVariableType.Value, TypeCode.String, false)
  {
    Value = value;
  }

  internal void SetValue(string value)
  {
    Value = value;
  }

  public override void SetValue(dynamic value)
  {
    Value = value;
  }

  public override string ApplyReplacementVariable(string contents)
  {
    contents = contents.Replace(this.Key, this.Value);
    contents = contents.Replace(this.Key.KeyComment(), this.Value.ValueComment());
    contents = contents.Replace(this.Key.KeyUppercaseUnderscore(), this.Value.ValueUppercaseUnderscore());
    contents = contents.Replace(this.Key.KeyUnderscore(), this.Value.ValueUnderscore());
    contents = contents.Replace(this.Key.KeyLowercase(), this.Value.ValueLowercase());
    return contents;
  }

  internal static List<ReplacementValue> CreateVariableListFromContents(string contents)
  {
    var replacementValues = new List<ReplacementValue>();
    string regexExpression = @"\$[a-zA-Z-0-9]+\$";
    MatchCollection matchedVariables = Regex.Matches(contents, regexExpression);

    for (int count = 0; count < matchedVariables.Count; count++)
    {
      var keyName = matchedVariables[count].Value;
      if (replacementValues.Any(x => x.Key == keyName) == false)
      {
        replacementValues.Add(new ReplacementValue(keyName));
      }
    }

    return replacementValues;
  }
}

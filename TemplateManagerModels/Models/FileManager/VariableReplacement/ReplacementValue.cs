using Microsoft.VisualBasic;
using Newtonsoft.Json;
using System.Text.RegularExpressions;
using TemplateManagerModels.Models.Helpers;

namespace TemplateManagerModels.Models.FileManager.VariableReplacement;

public class ReplacementValue : ReplacementVariableAbstract
{
  internal string? Value { get; private set; }
  internal string KeyComment => $"{this.Key.TrimEnd('$')}:comment$";
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

  internal override string ApplyReplacementVariable(string contents)
  {
    contents = contents.Replace(this.Key, this.Value);
    contents = contents.Replace(this.KeyComment, this.ValueComment);
    return contents;
  }

  internal static List<ReplacementValue> CreateVariableListFromContents(string contents)
  {
    var replacementValues = new List<ReplacementValue>();
    string regexExpression = @"\$[a-zA-Z-0-9]*\$";
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

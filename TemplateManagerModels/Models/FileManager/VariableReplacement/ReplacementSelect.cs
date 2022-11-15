using System.Text.RegularExpressions;
using System.Xml.Linq;
using TemplateManagerModels.Models.Enums;
using TemplateManagerModels.Models.Helpers;

namespace TemplateManagerModels.Models.FileManager.VariableReplacement;

internal class ReplacementSelect : ReplacementVariableAbstract
{
  internal string? Value { get; private set; }
  internal string[] Options { get; private set; }
  internal string IdentificationString { get; private set; }
  internal string KeyComment => $"$select:{this.Key.TrimEnd('$')}:comment$";
  internal string? ValueComment => Value?.AddSpacesToSentence() ?? string.Empty;

  public ReplacementSelect(string identificationString, string key, string[] options) :
    base(key, ReplacementVariableType.Select, TypeCode.String, true)
  {
    this.IdentificationString = identificationString;
    this.Options = options;
  }

  public override void SetValue(dynamic value)
  {
    Value = value;
  }

  internal static List<ReplacementSelect> CreateVariableListFromContents(string contents)
  {
    var replacementValues = new List<ReplacementSelect>();

    // Used to identify strings such as: $select:ContractType:Query|Request$
    string regexExpression = @"\$(select):([a-zA-Z-0-9]*):(([a-zA-Z-0-9<>]|)*(\||\$))*";
    MatchCollection matchedVariables = Regex.Matches(contents, regexExpression);

    for (int count = 0; count < matchedVariables.Count; count++)
    {
      Match selectMatch = matchedVariables[count];
      string keyName = selectMatch.Groups[2].Value;
      if (replacementValues.Any(x => x.Key == keyName) == false)
      {
        string[] optionsArray = selectMatch.Groups[3].Captures.Select(x => x.Value.Remove(x.Value.Length - 1)).ToArray();
        var replacementSelect = new ReplacementSelect(selectMatch.Value, keyName, optionsArray);
        replacementValues.Add(replacementSelect);
      }
    }

    return replacementValues;
  }

  public override string ApplyReplacementVariable(string contents)
  {
    contents = contents.Replace(this.IdentificationString, this.Value);
    contents = contents.Replace(this.KeyComment, this.ValueComment);
    return contents;
  }
}

using Microsoft.VisualBasic;
using System.Text.RegularExpressions;
using TemplateManagerModels.Models.Enums;

namespace TemplateManagerModels.Models.FileManager.VariableReplacement;

internal class ReplacementIf : ReplacementVariableAbstract, IReplacementVariable
{
  internal bool Value { get; private set; } = false;
  internal string IfString => $"$if:{Key}$";
  internal string ElseString => $"$endif:{Key}$";
  internal int IfLength => IfString.Length;
  internal int EndifLength => ElseString.Length;

  public ReplacementIf(string key) :
    base(key, ReplacementVariableType.If, TypeCode.Boolean, true)
  {
  }

  public override void SetValue(dynamic value)
  {
    Value = value;
  }

  internal static List<ReplacementIf> CreateVariableListFromContents(string contents)
  {
    var replacementValues = new List<ReplacementIf>();

    string regexExpressionStart = @"\$(if):([a-zA-Z-0-9]*)\$";
    MatchCollection matchedIfStarts = Regex.Matches(contents, regexExpressionStart);

    for (int count = 0; count < matchedIfStarts.Count; count++)
    {
      Match ifMatch = matchedIfStarts[count];
      string keyName = ifMatch.Groups[2].Value;
      if (replacementValues.Any(x => x.Key == keyName) == false)
      {
        var replacementIf = new ReplacementIf(keyName);
        replacementValues.Add(replacementIf);
      }
    }

    return replacementValues;
  }

  public override string ApplyReplacementVariable(string contents)
  {
    Dictionary<int, int> indexDictionary = GetIfVariableIndexDictionaryForContents(contents);
    contents = ApplyIfStatements(contents, indexDictionary);

    return contents;
  }

  /// <summary>
  /// Gets the start and end of each instance of an if statement and returns a dictionary of key being the start index and value being the end index.
  /// </summary>
  private Dictionary<int, int> GetIfVariableIndexDictionaryForContents(string contents)
  {
    Dictionary<int, int> indexDictionary = new();
    string regexExpressionStart = $@"\$(if):({this.Key})\$";
    string regexExpressionEnd = $@"\$(endif):({this.Key})\$";

    MatchCollection matchedIfStarts = Regex.Matches(contents, regexExpressionStart);
    MatchCollection matchedIfEnds = Regex.Matches(contents, regexExpressionEnd);

    for (int count = 0; count < matchedIfStarts.Count; count++)
    {
      Match firstMatch = matchedIfStarts[0];
      Match secondMatch = matchedIfEnds[0];

      indexDictionary.Add(firstMatch.Index, secondMatch.Index + secondMatch.Length);
    }

    return indexDictionary;
  }

  private string ApplyIfStatements(string contents, Dictionary<int, int> indexDictionary)
  {
    foreach (KeyValuePair<int, int> item in indexDictionary.OrderByDescending(x => x.Value))
    {
      if (this.Value)
      {
        contents = contents.Remove(item.Value - this.EndifLength, this.EndifLength);
        contents = contents.Remove(item.Key, this.IfLength);
      }
      else
      {
        contents = contents.Remove(item.Key, item.Value - item.Key);
      }
    }

    return contents;
  }
}

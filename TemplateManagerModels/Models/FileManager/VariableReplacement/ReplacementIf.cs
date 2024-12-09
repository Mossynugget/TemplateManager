using System.Text.RegularExpressions;
using TemplateManager.Models.Enums;

namespace TemplateManager.Models.FileManager.VariableReplacement;

internal class ReplacementIf : ReplacementVariableAbstract, IReplacementVariable
{
  internal bool Value { get; private set; } = false;
  private string ifSearch = "if";
  private string endIfSearch = "endif";
  internal string IfString => $"${ifSearch}:{Key}$";
  internal string EndIfString => $"${endIfSearch}:{Key}$";
  internal int IfLength => IfString.Length;
  internal int EndifLength => EndIfString.Length;

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

    string regexExpressionStart = $@"\$(if):([a-zA-Z-0-9]*)\$";
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
    contents = ApplyIfTruthy(contents);
    contents = ApplyIfFalsey(contents);

    return contents;
  }

  private string ApplyIfTruthy(string contents)
  {
    Dictionary<int, int> indexDictionary = GetIfVariableIndexDictionaryForContents(contents);
    contents = ApplyIfStatements(contents, indexDictionary);
    return contents;
  }

  private string ApplyIfFalsey(string contents)
  {
    this.Value = !this.Value;

    AlterIf("else");
    contents = ApplyIfTruthy(contents);
    AlterIf();

    this.Value = !this.Value;
    
    return contents;
  }

  private void AlterIf(string ifText = "if")
  {
    ifSearch = $"{ifText}";
    endIfSearch = $"end{ifText}";
  }

  /// <summary>
  /// Gets the start and end of each instance of an if statement and returns a dictionary of key being the start index and value being the end index.
  /// </summary>
  private Dictionary<int, int> GetIfVariableIndexDictionaryForContents(string contents)
  {
    Dictionary<int, int> indexDictionary = new();

    string regexExpressionStart = $@"\$({ifSearch}):({this.Key})\$";
    string regexExpressionEnd = $@"\$({endIfSearch}):({this.Key})\$";

    MatchCollection matchedIfStarts = Regex.Matches(contents, regexExpressionStart);
    MatchCollection matchedIfEnds = Regex.Matches(contents, regexExpressionEnd);

    for (int count = 0; count < matchedIfStarts.Count; count++)
    {
      Match firstMatch = matchedIfStarts[count];
      Match secondMatch = matchedIfEnds[count];

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

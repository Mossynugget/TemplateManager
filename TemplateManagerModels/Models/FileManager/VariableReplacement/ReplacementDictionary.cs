using System.Text.RegularExpressions;
using TemplateManagerModels.Models.Dtos;
using TemplateManagerModels.Models.Enums;

namespace TemplateManagerModels.Models.FileManager.VariableReplacement;

internal class ReplacementDictionary
{
  internal List<ReplacementValue> ReplacementValueList { get; set; }
  internal List<ReplacementIf> ReplacementIfList { get; set; }

  internal ReplacementDictionary(string? contents = null)
  {
    ReplacementValueList = new();
    ReplacementIfList = new();
    if (contents != null)
    {
      CreateReplacementList(contents);
    }
  }

  internal void AddReplacementValues(List<ReplacementValue>? replacementValues)
  {
    if((replacementValues != null) && replacementValues.Any())
      this.ReplacementValueList.AddRange(replacementValues);
  }

  internal string ReplaceContents(string contents)
  {
    contents = replaceIfContents(contents);
    contents = ReplaceValueContents(contents);

    return contents;
  }

  private string replaceIfContents(string contents)
  {
    for (int count = 0; count < ReplacementIfList.Count; count++)
    {
      ReplacementIf? replacementVariable = this.ReplacementIfList[count];
      Dictionary<int, int> indexDictionary = GetIfVariableIndexDictionaryForContents(contents, replacementVariable);
      contents = ApplyIfStatements(contents, replacementVariable, indexDictionary);
    }

    return contents;
  }

  /// <summary>
  /// Gets the start and end of each instance of an if statement and returns a dictionary of key being the start index and value being the end index.
  /// </summary>
  private static Dictionary<int, int> GetIfVariableIndexDictionaryForContents(string contents, ReplacementIf replacementVariable)
  {
    Dictionary<int, int> indexDictionary = new();
    string regexExpressionStart = $@"\$(if):({replacementVariable.Key})\$";
    string regexExpressionEnd = $@"\$(endif):({replacementVariable.Key})\$";

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

  private static string ApplyIfStatements(string contents, ReplacementIf replacementVariable, Dictionary<int, int> indexDictionary)
  {
    foreach (KeyValuePair<int, int> item in indexDictionary.OrderByDescending(x => x.Value))
    {
      if (replacementVariable.Value)
      {
        contents = contents.Remove(item.Value - replacementVariable.EndifLength, replacementVariable.EndifLength);
        contents = contents.Remove(item.Key, replacementVariable.IfLength);
      }
      else
      {
        contents = contents.Remove(item.Key, item.Value - item.Key);
      }
    }

    return contents;
  }

  public string ReplaceValueContents(string contents)
  {
    if (string.IsNullOrEmpty(contents))
    {
      return contents;
    }

    foreach (var replacementVariable in this.ReplacementValueList)
    {
      contents = contents.Replace(replacementVariable.Key, replacementVariable.Value);
      contents = contents.Replace(replacementVariable.KeyComment, replacementVariable.ValueComment);
    }

    return contents;
  }

  internal void CreateReplacementList(string contents)
  {
    CreateReplacementValueList(contents);
    CreatePlacementIfList(contents);
  }

  private void CreateReplacementValueList(string contents)
  {
    string regexExpression = @"\$[a-zA-Z-0-9]*\$";
    MatchCollection matchedVariables = Regex.Matches(contents, regexExpression);

    for (int count = 0; count < matchedVariables.Count; count++)
    {
      var keyName = matchedVariables[count].Value;
      if (ReplacementValueList.Any(x => x.Key == keyName) == false)
      {
        ReplacementValueList.Add(new ReplacementValue(keyName));
      }
    }
  }

  private void CreatePlacementIfList(string contents)
  {
    string regexExpressionStart = @"\$(if):([a-zA-Z-0-9]*)\$";
    MatchCollection matchedIfStarts = Regex.Matches(contents, regexExpressionStart);

    for (int count = 0; count < matchedIfStarts.Count; count++)
    {
      Match ifMatch = matchedIfStarts[count];
      var replacementIf = new ReplacementIf(ifMatch.Groups[2].Value);
      ReplacementIfList.Add(replacementIf);
    }
  }

  internal List<ReplacementVariableDto> GetReplacementVariablesAsDictionary()
  {
    var replacementVariableDtoList = new List<ReplacementVariableDto>();

    foreach (var replacementVariable in this.ReplacementValueList.Where(x => string.IsNullOrEmpty(x.Value)))
    {
      if (replacementVariableDtoList.Any(x => x.Key == replacementVariable.Key) == false)
      {
        replacementVariableDtoList.Add(new ReplacementVariableDto(replacementVariable));
      }
    }

    foreach (var replacementVariable in ReplacementIfList)
    {
      if (replacementVariableDtoList.Any(x => x.Key == replacementVariable.Key) == false)
      {
        replacementVariableDtoList.Add(new ReplacementVariableDto(replacementVariable));
      }
    }

    return replacementVariableDtoList;
  }

  internal void MapDictionaryToReplacementVariables(List<ReplacementVariableDto> replacementDictionary)
  {
    foreach (var replacementVariable in replacementDictionary)
    {
      switch (replacementVariable.ReplacementDictionaryType)
      {
        case ReplacementVariableType.Value:
          this.AddReplacementValueValue(replacementVariable.Key, replacementVariable.Value);
          break;
        case ReplacementVariableType.If:
          this.AddReplacementIfValue(replacementVariable.Key, replacementVariable.Value);
          break;
      }
    }
  }

  private void AddReplacementValueValue(string variableKey, string? variableValue)
  {
    if (variableValue == null)
    {
      return;
    }
    var replacementVariable = ReplacementValueList.Find(x => x.Key == variableKey);
    if (replacementVariable == null)
    {
      throw new InvalidOperationException("The specified replacement variable key doesn't exist.");
    }

    replacementVariable.SetValue(variableValue);
  }

  private void AddReplacementIfValue(string variableKey, bool? variableValue)
  {
    if (variableValue == null)
    {
      return;
    }

    foreach (var replacementIf in this.ReplacementIfList)
    {
      if (replacementIf.Key == variableKey)
      {
        replacementIf.SetValue(variableValue);
      }
    }
  }
}

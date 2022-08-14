using Microsoft.VisualBasic;
using System.Text.RegularExpressions;
using TemplateManagerModels.Models.Dtos;
using TemplateManagerModels.Models.Enums;
using TemplateManagerModels.Models.Helpers;

namespace TemplateManagerModels.Models.FileManager.VariableReplacement;

internal class ReplacementDictionary
{
  internal List<ReplacementValue> ReplacementVariableList { get; set; }
  internal List<ReplacementIf> ReplacementIfList { get; set; }

  internal ReplacementDictionary(string? contents = null)
  {
    ReplacementVariableList = new();
    ReplacementIfList = new();
    if (contents != null)
    {
      CreateReplacementList(contents);
    }
  }

  internal string ReplaceContents(string contents)
  {
    contents = replaceIfContents(contents);
    contents = replaceValueContents(contents);

    return contents;
  }

  private string replaceIfContents(string contents)
  {
    foreach (var replacementVariable in this.ReplacementIfList.OrderByDescending(x => x.IndexEnd))
    {
      if (replacementVariable.Value)
      {
        contents = contents.Remove(replacementVariable.IndexEnd - replacementVariable.EndifLength, replacementVariable.EndifLength);
        contents = contents.Remove(replacementVariable.IndexStart, replacementVariable.IfLength);
      }
      else
      {
        contents = contents.Remove(replacementVariable.IndexStart, replacementVariable.IndexEnd - replacementVariable.IndexStart);
      }
    }

    return contents;
  }

  private string replaceValueContents(string contents)
  {
    foreach (var replacementVariable in this.ReplacementVariableList)
    {
      contents = contents.Replace(replacementVariable.Key, replacementVariable.Value);
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
      if (ReplacementVariableList.Any(x => x.Key == keyName) == false)
      {
        ReplacementVariableList.Add(new ReplacementValue(keyName));
      }
    }
  }

  private void CreatePlacementIfList(string contents)
  {
    string regexExpressionStart = @"\$(if):([a-zA-Z-0-9]*)\$";
    string regexExpressionEnd = @"\$(endif):([a-zA-Z-0-9]*)\$";
    MatchCollection matchedIfStarts = Regex.Matches(contents, regexExpressionStart);
    MatchCollection matchedIfEnds = Regex.Matches(contents, regexExpressionEnd);

    for (int count = 0; count < matchedIfStarts.Count; count++)
    {
      Match firstMatch = matchedIfStarts[count];
      Match secondMatch = matchedIfEnds[count];
      var replacementIf = new ReplacementIf(firstMatch.Groups[2].Value, firstMatch.Index, secondMatch.Index + secondMatch.Length);
      ReplacementIfList.Add(replacementIf);
    }
  }

  internal List<ReplacementVariableDto> GetReplacementVariablesAsDictionary()
  {
    var replacementVariableDtoList = new List<ReplacementVariableDto>();

    foreach (var replacementVariable in this.ReplacementVariableList)
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
        case ReplacementVariableTypeEnum.VariableType:
          this.AddReplacementValue(replacementVariable.Key, replacementVariable.Value);
          break;
        case ReplacementVariableTypeEnum.IfType:
          this.AddReplacementIf(replacementVariable.Key, replacementVariable.Value);
          break;
      }
    }
  }

  private void AddReplacementValue(string variableKey, string? variableValue)
  {
    if (variableValue == null)
    {
      return;
    }
    var replacementVariable = ReplacementVariableList.Find(x => x.Key == variableKey);
    if (replacementVariable == null)
    {
      throw new InvalidOperationException("The specified replacement variable key doesn't exist.");
    }

    replacementVariable.SetValue(variableValue);
  }

  private void AddReplacementIf(string variableKey, bool? variableValue)
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

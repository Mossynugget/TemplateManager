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
      CreateReplacementLists(contents);
    }
  }

  internal void AddReplacementValues(List<ReplacementValue>? replacementValues)
  {
    if((replacementValues != null) && replacementValues.Any())
      this.ReplacementValueList.AddRange(replacementValues);
  }

  internal string ReplaceContents(string contents)
  {
    contents = ReplaceIfContents(contents);
    contents = ReplaceValueContents(contents);

    return contents;
  }

  private string ReplaceIfContents(string contents)
  {
    foreach (var replacementIf in this.ReplacementIfList)
    {
      ReplacementIf? replacementVariable = replacementIf;
      contents = replacementVariable.ApplyReplacementVariable(contents);
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
      contents = replacementVariable.ApplyReplacementVariable(contents);
    }

    return contents;
  }

  internal void CreateReplacementLists(string contents)
  {
    ReplacementValueList.AddRange(ReplacementValue.CreateVariableListFromContents(contents));
    ReplacementIfList.AddRange(ReplacementIf.CreateVariableListFromContents(contents));
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

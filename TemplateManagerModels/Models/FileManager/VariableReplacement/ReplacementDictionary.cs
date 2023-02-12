using TemplateManagerModels.Helpers;
using TemplateManagerModels.Models.Dtos;
using TemplateManagerModels.Models.Enums;

namespace TemplateManagerModels.Models.FileManager.VariableReplacement;

internal class ReplacementDictionary
{
  internal List<ReplacementValue> ReplacementValueList { get; set; }
  internal List<ReplacementIf> ReplacementIfList { get; set; }
  internal List<ReplacementSelect> ReplacementSelectList { get; set; }

  internal ReplacementDictionary(string? contents = null)
  {
    ReplacementValueList = new();
    ReplacementIfList = new();
    ReplacementSelectList = new();

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

  internal string ApplyAllReplaceLists(string contents)
  {
    if (string.IsNullOrEmpty(contents))
    {
      return contents;
    }

    contents = ReplaceIfContents(contents);
    contents = ReplaceValueContents(contents);
    contents = ReplaceSelectContents(contents);
    contents = contents.Replace("_$_", string.Empty);

    return contents;
  }

  private string ReplaceIfContents(string contents)
  {
    foreach (var replacementIf in this.ReplacementIfList)
    {
      contents = replacementIf.ApplyReplacementVariable(contents);
    }

    return contents;
  }

  public string ReplaceValueContents(string contents)
  {
    foreach (var replacementVariable in this.ReplacementValueList)
    {
      contents = replacementVariable.ApplyReplacementVariable(contents);
    }

    return contents;
  }

  public string ReplaceSelectContents(string contents)
  {
    foreach (var replacementVariable in this.ReplacementSelectList)
    {
      contents = replacementVariable.ApplyReplacementVariable(contents);
    }

    return contents;
  }

  internal void CreateReplacementLists(string contents)
  {
    if (string.IsNullOrEmpty(contents))
    {
      return;
    }

    var replacementValueList = ReplacementValue.CreateVariableListFromContents(contents);
    ReplacementValueList.AddRange(replacementValueList.Where(rl => ReplacementValueList.None(rvl => rvl.Key == rl.Key)));
    var replacementIfList = ReplacementIf.CreateVariableListFromContents(contents);
    ReplacementIfList.AddRange(replacementIfList.Where(rl => ReplacementIfList.None(rvl => rvl.Key == rl.Key)));
    var replacementSelectList = ReplacementSelect.CreateVariableListFromContents(contents);
    ReplacementSelectList.AddRange(replacementSelectList.Where(rl => ReplacementSelectList.None(rvl => rvl.Key == rl.Key)));
  }

  internal List<ReplacementVariableDto> GetReplacementVariablesAsDictionary()
  {
    var replacementVariableDtoList = new List<ReplacementVariableDto>();

    foreach (var replacementVariable in this.ReplacementValueList.Where(x => x.RequiresInput))
    {
      if (replacementVariableDtoList.Any(x => x.Key == replacementVariable.Key) == false)
      {
        replacementVariableDtoList.Add(new ReplacementVariableDto(replacementVariable));
      }
    }

    foreach (var replacementVariable in ReplacementIfList.Where(x => x.RequiresInput))
    {
      if (replacementVariableDtoList.Any(x => x.Key == replacementVariable.Key) == false)
      {
        replacementVariableDtoList.Add(new ReplacementVariableDto(replacementVariable));
      }
    }

    foreach (var replacementVariable in ReplacementSelectList.Where(x => x.RequiresInput))
    {
      if (replacementVariableDtoList.Any(x => x.Key == replacementVariable.Key) == false)
      {
        replacementVariableDtoList.Add(new ReplacementVariableDto(replacementVariable, replacementVariable.Options));
      }
    }

    return replacementVariableDtoList;
  }

  internal void MapDictionaryToReplacementVariables(List<ReplacementVariableDto> replacementDictionary)
  {
    foreach (var replacementVariableDto in replacementDictionary)
    {
      if (replacementVariableDto.Value == null) continue;

      ReplacementVariableAbstract replacementVariable; 

      switch (replacementVariableDto.ReplacementDictionaryType)
      {
        case ReplacementVariableType.If:
          replacementVariable = ReplacementIfList.First(findKeyPredicate(replacementVariableDto.Key));
          break;
        case ReplacementVariableType.Select:
          replacementVariable = ReplacementSelectList.First(findKeyPredicate(replacementVariableDto.Key));
          break;
        default:
          replacementVariable = ReplacementValueList.First(findKeyPredicate(replacementVariableDto.Key));
          break;
      }

      replacementVariable!.SetValue(replacementVariableDto.Value);
    }
  }

  private static Func<ReplacementVariableAbstract, bool> findKeyPredicate(string variableKey)
  {
    return x => x.Key == variableKey;
  }
}

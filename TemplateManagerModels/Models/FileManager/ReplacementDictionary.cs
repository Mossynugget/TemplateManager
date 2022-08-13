using System.Text.RegularExpressions;

namespace TemplateManagerModels.Models.FileManager;

internal class ReplacementDictionary
{
  internal List<ReplacementVariable> ReplacementVariableList { get; set; }

  internal ReplacementDictionary(string? contents = null)
  {
    ReplacementVariableList = new();
    if (contents != null)
    {
      this.CreateReplacementVariableList(contents);
    }
  }

  internal void CreateReplacementVariableList(string contents)
  {
    string regexExpression = @"\$[a-zA-Z-0-9]*\$";
    MatchCollection matchedVariables = Regex.Matches(contents, regexExpression);

    for (int count = 0; count < matchedVariables.Count; count++)
    {
      this.addReplacementVariableKey(matchedVariables[count].Value);
    }
  }

  private void addReplacementVariableKey(string keyName)
  {
    if (ReplacementVariableList.Any(x => x.Key == keyName) == false)
    {
      ReplacementVariableList.Add(new ReplacementVariable(keyName));
    }
  }

  internal void AddReplacementValue(string variableKey, string? variableValue)
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

  internal Dictionary<string, string?> GetReplacementVariablesAsDictionary()
  {
    IDictionary<string, string?> result = new Dictionary<string, string?>();
    foreach (var replacementVariable in this.ReplacementVariableList)
    {
      result.Add(replacementVariable.ToKeyValuePair());
    }

    return (Dictionary<string, string?>)result;
  }

  internal void MapDictionaryToReplacementVariables(Dictionary<string, string?> replacementDictionary)
  {
    foreach (var keyValuePair in replacementDictionary)
    {
      this.AddReplacementValue(keyValuePair.Key, keyValuePair.Value);
    }
  }
}

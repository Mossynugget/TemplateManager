using System.Text.RegularExpressions;

namespace TemplateManagerModels.Models.FileManager;

public class ReplacementDictionary
{
  private List<ReplacementVariable> ReplacementVariableList { get; set; }

  internal ReplacementDictionary()
  {
    ReplacementVariableList = new();
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

  public void AddReplacementValue(string variableKey, string variableValue)
  {
    var replacementVariable = ReplacementVariableList.Find(x => x.Key == variableKey);
    if (replacementVariable == null)
    {
      throw new InvalidOperationException("The specified replacement variable key doesn't exist.");
    }

    replacementVariable.SetValue(variableValue);
  }
}

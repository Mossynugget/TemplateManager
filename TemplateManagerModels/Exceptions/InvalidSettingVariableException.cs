namespace TemplateManager.Models.Exceptions
{
  /// <summary>
  /// The invalid location variable exception used when there was a problem finding a location based variable.
  /// </summary>
  public class InvalidSettingVariableException : Exception
  {
    /// <summary>
    /// The default constructor for an exception thrown when a variable is invalid.
    /// </summary>
    /// <param name="message"></param>
    public InvalidSettingVariableException(string message) :
      base(message) 
    { 
    }
  }
}

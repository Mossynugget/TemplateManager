namespace TemplateManagerModels.Exceptions
{
  /// <summary>
  /// The invalid variable exception.
  /// </summary>
  public class InvalidVariableException : Exception
  {
    /// <summary>
    /// The default constructor for an exception thrown when a variable is invalid.
    /// </summary>
    /// <param name="message"></param>
    public InvalidVariableException(string message) :
      base(message) 
    { 
    }
  }
}

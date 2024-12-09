using TemplateManager.Models.Exceptions;

namespace TemplateManager.Cli.ExceptionHandler
{
  internal static class ExceptionHandler
  {
    internal static void HandleException(Exception ex)
    {
      switch(ex)
      {
        case InvalidVariableException e:
          Console.WriteLine(e.Message);
          break;
        case InvalidOperationException e:
          Console.WriteLine(e.Message);
          break;
        case InvalidSettingVariableException e:
          Console.WriteLine(e.Message);
          break;
        default:
          Console.WriteLine(ex.Message);
          break;
      }
    }
  }
}

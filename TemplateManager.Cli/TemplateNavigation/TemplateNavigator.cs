using Microsoft.VisualBasic;

namespace TemplateManager.Cli.TemplateNavigation;

internal class TemplateNavigator
{
  private string baseDirectory;
  private DirectoryNavigator? directoryNavigator;

  public TemplateNavigator(string baseDirectory = "D:\\CodeTemplates\\")
  {
    this.baseDirectory = baseDirectory;
  }

  public async Task<string> SelectTemplate()
  {
    await this.validateFolderExists();
    this.directoryNavigator = new(this.baseDirectory);

    Console.WriteLine($"Please navigate to the template you wish to implement.");

    this.templateNavigation();
    return this.directoryNavigator.path;
  }

  private async Task validateFolderExists()
  {
    if (Directory.Exists(this.baseDirectory))
    {
      return;
    }

    Console.WriteLine($"The base directory \"({this.baseDirectory})\" doesn't exist, would you like us to create it for you?\n (y) for yes and (n) for no.");
    var yesNo = Console.ReadLine();
    if (yesNo?.StartsWith("y", StringComparison.OrdinalIgnoreCase) ?? false)
    {
      Directory.CreateDirectory(this.baseDirectory);
      var templateExample = File.ReadAllText("TemplateNavigation\\TemplateExample\\TemplateOne.cs");
      await File.WriteAllTextAsync($"{this.baseDirectory}ExampleFile.cs", templateExample).ConfigureAwait(false);
      return;
    }
    throw new InvalidOperationException("No directory present.");
  }

  private void templateNavigation()
  {
    while(this.directoryNavigator!.isTemplateInd == false)
    {
      WriteOptions();

      int navigationIndex = int.Parse(Console.ReadLine() ?? "-1");

      if (navigationIndex == -1)
      {
        if (this.directoryNavigator.ParentNavigation == null)
        {
          throw new InvalidOperationException("No template selected");
        }
        this.directoryNavigator = this.directoryNavigator.ParentNavigation;
      }
      else
      {
        this.directoryNavigator = this.directoryNavigator.navigationList[navigationIndex];
      }
    }
  }

  private void WriteOptions()
  {
    if (this.directoryNavigator!.ParentNavigation == null)
    {
      Console.WriteLine($"[-1]: Exit");
    }
    else
    {
      Console.WriteLine($"[-1]: Go Back");
    }

    for (int i = 0; i < this.directoryNavigator.navigationList.Count; i++)
    {
      Console.WriteLine($"[{i}]: {this.directoryNavigator.navigationList[i].name}");
    }
  }
}

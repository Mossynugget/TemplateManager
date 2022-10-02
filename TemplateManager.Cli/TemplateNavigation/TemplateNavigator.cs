using TemplateManager.Cli.TemplateNavigation.DTOs;

namespace TemplateManager.Cli.TemplateNavigation;

internal class TemplateNavigator
{
  private string _baseDirectory = "C:\\CodeTemplates\\";
  private string _templateCollectionsFileName = "TemplateCollections.json";
  private string _templateCollectionsPath;
  private DirectoryNavigator? directoryNavigator;

  public TemplateNavigator(string? baseDirectory = null)
  {
    _baseDirectory = baseDirectory ?? _baseDirectory;
    _templateCollectionsPath = Path.Combine(_baseDirectory, _templateCollectionsFileName);
}

  public async Task<string> SelectTemplate()
  {
    await this.validateFolderExists();
    var templateCollections = TemplateCollection.GenerateTemplateCollection(_templateCollectionsPath);
    this.directoryNavigator = new(templateCollections);

    Console.WriteLine($"Please navigate to the template you wish to implement.");

    this.templateNavigation();
    return this.directoryNavigator.path;
  }

  private async Task validateFolderExists()
  {
    if (Directory.Exists(_baseDirectory))
    {
      return;
    }

    Console.WriteLine($"The base directory \"({_baseDirectory})\" doesn't exist, would you like us to create it for you?\n (y) for yes and (n) for no.");
    var yesNo = Console.ReadLine();
    if (yesNo?.StartsWith("y", StringComparison.OrdinalIgnoreCase) ?? false)
    {
      Directory.CreateDirectory(_baseDirectory);
      var templateExample = File.ReadAllText("TemplateNavigation\\TemplateExample\\TemplateOne.cs");
      await File.WriteAllTextAsync($"{_baseDirectory}ExampleFile.cs", templateExample).ConfigureAwait(false);
      return;
    }
    throw new InvalidOperationException("No directory present.");
  }

  private void templateNavigation()
  {
    while(this.directoryNavigator!.isTemplateInd == false)
    {
      WriteOptions();

      int navigationIndex = int.Parse(Console.ReadLine()) - 1;

      if (navigationIndex == -2
        && CanGetAllTemplates())
      {
        this.directoryNavigator = this.directoryNavigator.GetAllTemplateOptions();
      }
      else if (navigationIndex == -1)
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
    Console.WriteLine($"\n{this.directoryNavigator!.path}");
    if (this.directoryNavigator!.ParentNavigation == null)
    {
      Console.WriteLine($"[0]: Exit");
    }
    else
    {
      Console.WriteLine($"[0]: Go Back");
    }

    for (int i = 1; i < this.directoryNavigator.navigationList.Count + 1; i++)
    {
      Console.WriteLine($"[{i}]: {this.directoryNavigator.navigationList[i - 1].name}");
    }

    if (CanGetAllTemplates())
    {
      Console.WriteLine($"[-1]: Get All Options");
    }
  }

  private bool CanGetAllTemplates()
  {
    return this.directoryNavigator!.ParentNavigation != null
          && this.directoryNavigator.ContainsAllTemplateOptionsInd == false
          && this.directoryNavigator.navigationList.Any(x => x.isTemplateInd == false);
  }
}

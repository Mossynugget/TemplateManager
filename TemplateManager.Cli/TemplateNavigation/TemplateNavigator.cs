using Sharprompt;
using TemplateManager.Cli.TemplateNavigation.DTOs;

namespace TemplateManager.Cli.TemplateNavigation;

internal class TemplateNavigator
{
  private string _baseDirectory = "C:\\CodeTemplates\\";
  private string _templateCollectionsFileName = "TemplateCollections.tmplt";
  private string _templateCollectionsPath;
  private DirectoryNavigator? directoryNavigator;
  private const string exit = "Exit";
  private const string goBack = "Go Back";
  private const string showAllOptions = "Show All Options";

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
      await GenerateExampleFiles().ConfigureAwait(false);
      return;
    }
    throw new InvalidOperationException("No directory present."); 
  }

  private async Task GenerateExampleFiles()
  {
    Directory.CreateDirectory(_baseDirectory);
    Directory.CreateDirectory($"{_baseDirectory}\\Examples\\");
    var directoryCollection = File.ReadAllText("TemplateNavigation\\TemplateExample\\TemplateCollections.tmplt");
    var templateExample = File.ReadAllText("TemplateNavigation\\TemplateExample\\TemplateOne.cs");
    await File.WriteAllTextAsync($"{_baseDirectory}TemplateCollections.tmplt", directoryCollection).ConfigureAwait(false);
    await File.WriteAllTextAsync($"{_baseDirectory}\\Examples\\ExampleFile.cs", templateExample).ConfigureAwait(false);
  }

  private void templateNavigation()
  {
    while(this.directoryNavigator!.isTemplateInd == false)
    {
      string[] options = GetOptions();
      var template = Prompt.Select("Select your template", options);

      if (template == showAllOptions
        && CanGetAllTemplates())
      {
        this.directoryNavigator = this.directoryNavigator.GetAllTemplateOptions();
      }
      else if (template == exit)
      {
        throw new InvalidOperationException("No template selected");
      }
      else if (template == goBack)
      {
        this.directoryNavigator = this.directoryNavigator.ParentNavigation;
      }
      else
      {
        this.directoryNavigator = this.directoryNavigator.navigationList.Find(x => x.name == template);
      }
    }
  }
  
  private string[] GetOptions()
  {
    List<string> options = new();
    Console.WriteLine($"\n{this.directoryNavigator!.path}");
    if (this.directoryNavigator!.ParentNavigation == null)
    {
      options.Add(exit);
    }
    else
    {
      options.Add(goBack);
    }

    for (int i = 1; i < this.directoryNavigator.navigationList.Count + 1; i++)
    {
      options.Add($"{this.directoryNavigator.navigationList[i - 1].name}");
    }

    if (CanGetAllTemplates())
    {
      options.Add(showAllOptions);
    }

    return options.ToArray();
  }

  private bool CanGetAllTemplates()
  {
    return this.directoryNavigator!.ParentNavigation != null
          && this.directoryNavigator.ContainsAllTemplateOptionsInd == false
          && this.directoryNavigator.navigationList.Any(x => x.isTemplateInd == false);
  }
}

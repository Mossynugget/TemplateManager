using Sharprompt;
using System.Reflection;
using TemplateManager.Cli.TemplateNavigation.DTOs;
using TemplateManagerModels.Helpers;
using TemplateManagerModels.Models.FileManager.FileReplacementHelpers;

namespace TemplateManager.Cli.TemplateNavigation;

internal class TemplateNavigator
{
  private string _baseDirectory = PathExtensions.GetRoot();
  private string _rootFolderName = "CodeTemplates";
  private string _templateCollectionsFileName = "TemplateCollections.tmplt";
  private string _templateCollectionsPath => Path.Combine(_baseDirectory, _templateCollectionsFileName);
  private DirectoryNavigator? directoryNavigator;
  private const string exit = "Exit";
  private const string goBack = "Go Back";
  private const string showAllOptions = "Show All Options";
  private const string navigateToRoot = "Navigate To Root";

  public TemplateNavigator(string? baseDirectory = null)
  {
    calculateBaseDirectory(baseDirectory);
  }

  private void calculateBaseDirectory(string? baseDirectory = null)
  {
    if (string.IsNullOrEmpty(baseDirectory) == false)
    {
      _baseDirectory = baseDirectory;
    }
    try
    {
      _baseDirectory = Environment.CurrentDirectory.FindDirectory(_rootFolderName);
    }
    catch { }
  }

  public async Task<string> SelectTemplate()
  {
    await this.validateFolderExists();
    var templateCollections = TemplateCollection.GenerateTemplateCollection(_templateCollectionsPath);
    if (templateCollections != null)
    {
      this.directoryNavigator = new(templateCollections);
    }
    else
    {
      this.directoryNavigator = new(_baseDirectory);
    }

    Console.WriteLine($"Please navigate to the template you wish to implement.");

    await this.templateNavigation();
    return this.directoryNavigator.path;
  }

  private async Task validateFolderExists()
  {
    if (Directory.Exists(_baseDirectory))
    {
      return;
    }

    var yesNo = Prompt.Confirm($"The base directory \"({_baseDirectory})\" doesn't exist, would you like us to create it for you?");
    if (yesNo)
    {
      await GenerateExampleFiles().ConfigureAwait(false);
      return;
    }
    throw new InvalidOperationException("No directory present.");
  }

  private async Task GenerateExampleFiles()
  {
    Directory.CreateDirectory($"{_baseDirectory}\\Examples\\");

    var assembly = Assembly.GetExecutingAssembly();
    var test = Assembly.GetExecutingAssembly().GetManifestResourceNames();

    this.WriteFileToDestination("TemplateManager.Cli.TemplateNavigation.TemplateExample.TemplateCollections.tmplt"
        , $"{_baseDirectory}TemplateCollections.tmplt");

    this.WriteFileToDestination("TemplateManager.Cli.TemplateNavigation.TemplateExample.TemplateOne.cs"
        , $"{_baseDirectory}\\Examples\\ExampleFile.cs");
  }

  private void WriteFileToDestination(string resourceName, string destination)
  {
    Stream s = Assembly.GetExecutingAssembly().GetManifestResourceStream(resourceName);
    FileStream resourceFile = new FileStream(destination, FileMode.Create);

    byte[] b = new byte[s.Length + 1];
    s.Read(b, 0, Convert.ToInt32(s.Length));
    resourceFile.Write(b, 0, Convert.ToInt32(b.Length - 1));
    resourceFile.Flush();
    resourceFile.Close();

    resourceFile = null;
  }

  private async Task templateNavigation()
  {
    while (this.directoryNavigator!.isTemplateInd == false)
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
      else if (template == navigateToRoot)
      {
        _baseDirectory = PathExtensions.GetRoot();
        await SelectTemplate();
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

    for (int i = 1; i < this.directoryNavigator.navigationList.Count + 1; i++)
    {
      options.Add($"{this.directoryNavigator.navigationList[i - 1].name}");
    }

    if (CanGetAllTemplates())
    {
      options.Add(showAllOptions);
    }

    if (this.directoryNavigator!.ParentNavigation == null)
    {
      options.Add(exit);
    }
    else
    {
      options.Add(goBack);
    }

    options.Add(navigateToRoot);

    return options.ToArray();
  }

  private bool CanGetAllTemplates()
  {
    return this.directoryNavigator!.ParentNavigation != null
          && this.directoryNavigator.ContainsAllTemplateOptionsInd == false
          && this.directoryNavigator.navigationList.Any(x => x.isTemplateInd == false);
  }
}

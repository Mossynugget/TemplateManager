using TemplateManager.Cli.TemplateNavigation.DTOs;

namespace TemplateManager.Cli.TemplateNavigation;

internal class DirectoryNavigator
{
  internal DirectoryNavigator? ParentNavigation;
  internal readonly bool isTemplateInd = false;
  internal List<DirectoryNavigator> navigationList;
  internal string? path;
  internal string name => nameOverride ?? Path.GetFileName(path) + (this.isTemplateInd ? string.Empty : "/");
  internal string? nameOverride { get; set; }
  internal bool ContainsAllTemplateOptionsInd = false;

  internal DirectoryNavigator(List<TemplateCollection> templateCollections)
  {
    navigationList = new List<DirectoryNavigator>();
    this.nameOverride = "Route";
    foreach (var templateCollection in templateCollections)
    {
      this.navigationList.Add(new(templateCollection.Path, this, nameOverride: templateCollection.Name));
    }
  }

  internal DirectoryNavigator(string path, DirectoryNavigator? parentNavigation = null, bool isTemplateInd = false, string? nameOverride = null) {
    navigationList = new List<DirectoryNavigator>();
    this.path = path;
    ParentNavigation = parentNavigation;
    this.isTemplateInd = isTemplateInd;
    this.nameOverride = nameOverride;
    loadNavigationList();
  }

  private DirectoryNavigator(DirectoryNavigator currentNavigation, bool containsAllFileOptions)
  {
    navigationList = new List<DirectoryNavigator>();
    this.path = currentNavigation.path;
    ParentNavigation = currentNavigation;
    this.ContainsAllTemplateOptionsInd = containsAllFileOptions;
    loadAllTemplates();
  }

  /// <summary>
  /// Method used to get all of the potential files.
  /// </summary>
  public DirectoryNavigator GetAllTemplateOptions()
  {
    return new DirectoryNavigator(this, true);
  }

  private void loadNavigationList()
  {
    if (this.isTemplateInd)
    {
      return;
    }

    foreach (var file in Directory.GetFiles(this.path, "*", SearchOption.TopDirectoryOnly))
    {
      this.navigationList.Add(new DirectoryNavigator(file, this, true));
    }

    foreach (var file in Directory.GetDirectories(this.path, "*", SearchOption.TopDirectoryOnly))
    {
      this.navigationList.Add(new DirectoryNavigator(file, this));
    }
  }


  /// <summary>
  /// Method used to get all of the potential files.
  /// </summary>
  private void loadAllTemplates()
  {
    this.navigationList = new();
    foreach (var file in Directory.GetFiles(this.path, "*", SearchOption.AllDirectories))
    {
      this.navigationList.Add(new DirectoryNavigator(file, this, true));
    }
  }
}

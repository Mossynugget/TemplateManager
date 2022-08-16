namespace TemplateManager.Cli.TemplateNavigation;

internal class DirectoryNavigator
{
  internal DirectoryNavigator? ParentNavigation;
  internal readonly bool isTemplateInd = false;
  internal List<DirectoryNavigator> navigationList;
  internal string path;
  internal string name => Path.GetFileName(path) + (this.isTemplateInd ? string.Empty : "/");
  internal bool ContainsAllTemplateOptionsInd = false;

  internal DirectoryNavigator(string path, DirectoryNavigator? parentNavigation = null, bool isTemplateInd = false) {
    navigationList = new List<DirectoryNavigator>();
    this.path = path;
    ParentNavigation = parentNavigation;
    this.isTemplateInd = isTemplateInd;
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

    foreach (var file in Directory.GetDirectories(this.path, "*", SearchOption.TopDirectoryOnly))
    {
      this.navigationList.Add(new DirectoryNavigator(file, this));
    }

    foreach (var file in Directory.GetFiles(this.path, "*", SearchOption.TopDirectoryOnly))
    {
      this.navigationList.Add(new DirectoryNavigator(file, this, true));
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

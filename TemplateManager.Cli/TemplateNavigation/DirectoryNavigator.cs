namespace TemplateManager.Cli.TemplateNavigation;

internal class DirectoryNavigator
{
  internal DirectoryNavigator? ParentNavigation;
  internal readonly bool isTemplateInd;
  internal List<DirectoryNavigator> navigationList;
  internal string path;
  internal string name => this.isTemplateInd ? Path.GetFileName(path) : Path.GetDirectoryName(path);

  internal DirectoryNavigator(string path, DirectoryNavigator? parentNavigation = null, bool isTemplateInd = false) {
    navigationList = new List<DirectoryNavigator>();
    this.path = path;
    ParentNavigation = parentNavigation;
    this.isTemplateInd = isTemplateInd;
    loadNavigationList();
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
}

using Newtonsoft.Json;

namespace TemplateManager.Cli.TemplateNavigation.DTOs
{
  internal class TemplateCollection
  {
    /// <summary>
    /// Gets or sets the name of the template collection.
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    /// Gets or sets the path of the template collection.
    /// </summary>
    public string Path { get; set; }

    public static List<TemplateCollection> GenerateTemplateCollection(string TemplateCollectionsLocation)
    {
      using (StreamReader r = new StreamReader(TemplateCollectionsLocation))
      {
        string json = r.ReadToEnd();
        List<TemplateCollection> templateCollections = JsonConvert.DeserializeObject<List<TemplateCollection>>(json);
        return templateCollections;
      }
    }
  }
}

namespace TemplateManagerModels.Models.FileManager;

internal class FileTemplate
{
  internal string TemplateFilePath;
  internal string Name;
  internal string FileExtension;
  internal string Contents;
  internal string Destination = "D:/Clients/ItemTemplateMauiApp/ItemTemplateCreatorApi/ItemTemplateCreatorApi/TestLocation/";

  internal FileTemplate(string templateFilePath)
  {
    TemplateFilePath = templateFilePath;
    Name = templateFilePath;
  }

  internal async void GenerateFileAsync(string? destination)
  {
    if (destination != null)
    {
      this.SetDestination(destination);
    }

    string finalPath = Path.Combine(Destination, Name);
    finalPath = Path.ChangeExtension(finalPath, FileExtension);
    await File.WriteAllTextAsync(finalPath, Contents);
  }

  internal void SetDestination(string destination)
  {
    this.Destination = destination;
  }
}

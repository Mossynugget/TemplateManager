namespace TemplateManager.Cli;

using System.Threading.Tasks;
using TemplateManagerModels.Models;
using TemplateManagerModels.Models.Helpers;

class TestClass
{
  static async Task Main(string[] args)
  {
    try
    {
      string selectedFile = SelectFile();

      TemplateFileGenerator templateFileGenerator = new(selectedFile);

      GetFileSettingsValues(templateFileGenerator);
      GetReplacementDictionaryValues(templateFileGenerator);
      await templateFileGenerator.GenerateFiles().ConfigureAwait(false);
      Console.WriteLine($"file(s) generated.");
    }
    // Used as an exit method.
    catch (InvalidOperationException ex)
    {
      Console.WriteLine($"{ex.Message}.");
    }

    static string SelectFile()
    {
      string baseDirectory = "D:\\Clients\\ItemTemplateMauiApp\\ItemTemplateCreatorApi\\TemplateManager.Cli\\ItemTemplates\\"; // This will be updated to an appsetting
      string currentDirectory = baseDirectory;
      List<string> navigationOptions = GetNavigationList(currentDirectory);

      string selectedFile = string.Empty;

      Console.WriteLine($"Please navigate to the template you wish to implement.");

      do
      {
        for (int i = 0; i < navigationOptions.Count; i++)
        {
          Console.WriteLine($"[{i}]: {navigationOptions[i]}");
        }

        int navigationIndex = int.Parse(Console.ReadLine());

        if (navigationIndex == 0)
        {
          NavigateBack(ref navigationOptions, ref currentDirectory, baseDirectory, navigationIndex);
        }
        else
        {
          currentDirectory = NavigateForward(ref navigationOptions, ref selectedFile, navigationIndex);
        }
      } while (selectedFile == string.Empty);


      return selectedFile;
    }

    static List<string> GetNavigationList(string currentDirectory)
    {
      List<string> navigationOptions = new() { "Back" };
      navigationOptions.AddRange(Directory.GetDirectories(currentDirectory, "*", SearchOption.TopDirectoryOnly));
      navigationOptions.AddRange(Directory.GetFiles(currentDirectory, "*", SearchOption.TopDirectoryOnly));
      return navigationOptions;
    }

    static string NavigateForward(ref List<string> navigationOptions, ref string selectedFile, int navigationIndex)
    {
      string currentDirectory = navigationOptions[navigationIndex];
      if (File.GetAttributes(currentDirectory) == FileAttributes.Directory)
      {
        navigationOptions = GetNavigationList(currentDirectory);
      }
      else
      {
        selectedFile = currentDirectory;
      }

      return currentDirectory;
    }

    static void NavigateBack(ref List<string> navigationOptions, ref string currentDirectory, string baseDirectory, int navigationIndex)
    {
      if (currentDirectory == baseDirectory)
      {
        throw new InvalidOperationException("Exited at file select.");
      }

      currentDirectory = currentDirectory.Substring(0, currentDirectory.LastIndexOf('/') + 1);

      navigationOptions = GetNavigationList(currentDirectory);
    }

    static void GetFileSettingsValues(TemplateFileGenerator templateFileGenerator)
    {
      var fileSettingsDtoList = templateFileGenerator.GetFileTemplateSettings();

      if (fileSettingsDtoList.Count == 1)
      {
        // fileSettingsDtoList[0].Destination = Environment.CurrentDirectory;
      }

      foreach (var fileSettingsDto in fileSettingsDtoList)
      {
        Console.WriteLine($"Please provide a file name for {fileSettingsDto.TemplateName}.");
        fileSettingsDto.FileName = Console.ReadLine();
      }

      templateFileGenerator.MapFileTemplateSettings(fileSettingsDtoList);
    }

    static void GetReplacementDictionaryValues(TemplateFileGenerator templateFileGenerator)
    {
      var replacementDictionary = templateFileGenerator.GetReplacementDictionary();

      foreach (var replacement in replacementDictionary)
      {
        if (replacement.allowedType == TypeCode.Boolean)
        {
          Console.WriteLine($"Would you like to {replacement.Key.Replace("$", "").AddSpacesToSentence()}?\nPlease use 'y' for yes and 'n' for no.");
        }
        else
        {
          Console.WriteLine($"Please enter a value for {replacement.Key.Replace("$", "").AddSpacesToSentence()}");
        }
        replacement.SetValue(Console.ReadLine());
      }

      templateFileGenerator.MapReplacementDictionary(replacementDictionary);
    }
  }
}
// See https://aka.ms/new-console-template for more information
using ItemTemplateCreatorApi.Helpers;
using System.Text.RegularExpressions;

try
{
  string selectedFile = SelectFile();
  string contents = LoadFileContents(selectedFile);
  string fileName = SelectFileName();
  contents = ReplaceFileNameInFile(selectedFile, contents, fileName);
  Dictionary<string, string> replacementDictionary = LoadReplacementDictionary(contents);
  GetReplacementDictionary(replacementDictionary);
  contents = ReplaceVariablesInFile(contents, replacementDictionary);
  await GenerateFile(contents, fileName);
}
// Used as an exit method.
catch (InvalidOperationException ex)
{
  Console.WriteLine($"{ex.Message} \nThank you, come again.");
}

static string SelectFile()
{
  string baseDirectory = "./ItemTemplates/"; // This will be updated to an appsetting
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

static string LoadFileContents(string selectedFile)
{
  return File.ReadAllText(selectedFile);
}

static string SelectFileName()
{
  Console.WriteLine($"Please provide a file name.");
  var fileName = Console.ReadLine();
  return fileName;
}

static string ReplaceFileNameInFile(string selectedFile, string contents, string fileName)
{
  contents = contents.Replace(Path.GetFileNameWithoutExtension(selectedFile), fileName);
  return contents;
}

static Dictionary<string, string> LoadReplacementDictionary(string contents)
{
  string regexExpression = @"\$[a-zA-Z-0-9]*\$";
  MatchCollection matchedVariables = Regex.Matches(contents, regexExpression);
  Dictionary<string, string> replacementDictionary = new();

  for (int count = 0; count < matchedVariables.Count; count++)
  {
    replacementDictionary[matchedVariables[count].Value] = string.Empty;
  }

  return replacementDictionary;
}

static void GetReplacementDictionary(Dictionary<string, string> replacementDictionary)
{
  foreach (var replacement in replacementDictionary)
  {
    Console.WriteLine($"Please enter a value for {replacement.Key.Replace("$", "").AddSpacesToSentence()}");
    replacementDictionary[replacement.Key] = Console.ReadLine();
  }
}

static string ReplaceVariablesInFile(string contents, Dictionary<string, string> replacementDictionary)
{
  foreach (var replacement in replacementDictionary)
  {
    contents = contents.Replace(replacement.Key, replacement.Value);
  }

  return contents;
}

static async Task GenerateFile(string contents, string fileName)
{
  await File.WriteAllTextAsync($"D:/Clients/ItemTemplateMauiApp/ItemTemplateCreatorApi/ItemTemplateCreatorApi/TestLocation/{fileName}.cs", contents);
}
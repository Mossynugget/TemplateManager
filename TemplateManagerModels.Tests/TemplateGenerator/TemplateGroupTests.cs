using System.Reflection;
using TemplateManager.Models.Helpers;

namespace TemplateManager.Models.Tests.TemplateGenerator;

public class TemplateGroupTests
{
  private readonly string testFilePath = $"TemplateGenerator\\TestFileGroup".GetPath();
  private readonly string testGroupFileName = "TestGroup.tmplt";
  private readonly string inputFileName = "InputFile.txt";
  private readonly string programCsFileName = "program.cs";
  private string programCsToCopy => $"{testFilePath}\\TestFilesToCopy\\{programCsFileName}";
  private string expectedResultsFolderPath => $"{testFilePath}\\ExpectedResults\\";
  private readonly string testResultFileName = "TestResult.txt";

  [Fact]
  public async Task FileGroupTest()
  {
    string testResultPath = Path.Combine(testFilePath, Guid.NewGuid().ToString().Replace("-", string.Empty));

    string testFile = getTestFilePath(testFilePath, testGroupFileName);
    Directory.CreateDirectory(testResultPath);

    File.Copy(programCsToCopy, $"{testResultPath}\\{programCsFileName}", true);

    TemplateFileGenerator templateFileGenerator = new(testFile);
    var fileSettingsDtoList = templateFileGenerator.GetFileTemplateSettings();
    fileSettingsDtoList[0].Destination = getTestFilePath(testResultPath, string.Empty);
    fileSettingsDtoList[0].FileName = Path.GetFileNameWithoutExtension(testResultFileName);

    fileSettingsDtoList[1].Destination = getTestFilePath(testResultPath, string.Empty);
    fileSettingsDtoList[1].FileName = Path.GetFileNameWithoutExtension(programCsFileName);
    templateFileGenerator.MapFileTemplateSettings(fileSettingsDtoList);

    var replacementDictionary = templateFileGenerator.GetReplacementDictionary();

    replacementDictionary[0].SetValue("TestOutput");
    replacementDictionary[1].SetValue("AlsoTestOutput");
    replacementDictionary[2].SetValue(true);
    replacementDictionary[3].SetValue(false);
    replacementDictionary[4].SetValue("Option2");

    templateFileGenerator.MapReplacementDictionary(replacementDictionary);

    try
    {
      await templateFileGenerator.GenerateFiles();

      string programExpectedResult = getFileContents(Path.Combine(expectedResultsFolderPath, programCsFileName).GetPath());
      string programResult = getFileContents(Path.Combine(testResultPath, programCsFileName).GetPath());
      string inputExpectedResult = getFileContents(Path.Combine(expectedResultsFolderPath, inputFileName).GetPath());
      string outputResult = getFileContents(Path.Combine(testResultPath, testResultFileName).GetPath());

      Assert.Equal(programExpectedResult, programResult);
      Assert.Equal(inputExpectedResult, outputResult);
    }
    finally
    {
      Directory.Delete(testResultPath, true);
    }
  }

  private static string getFileContents(string stringTest)
  {
    StreamReader reader = new StreamReader(stringTest);
    var fileContents = reader.ReadToEnd();
    reader.Dispose();
    return fileContents;
  }

  private static string getTestFilePath(string relativePath, string fileName)
  {
    var codeBaseUrl = new Uri(Assembly.GetExecutingAssembly().Location);
    var codeBasePath = Uri.UnescapeDataString(codeBaseUrl.AbsolutePath);
    var dirPath = Path.GetDirectoryName(codeBasePath);
    return Path.Combine(dirPath, relativePath, fileName);
  }
}
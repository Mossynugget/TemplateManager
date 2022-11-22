using Microsoft.VisualStudio.TestPlatform.ObjectModel;
using System.Reflection;
using TemplateManagerModels.Helpers;
using TemplateManagerModels.Models;

namespace TemplateManagerModels.Tests.TemplateGenerator;

public class TemplateFileGeneratorTests
{
  private readonly string testFilePath = "TemplateGenerator\\TestFileGenerator".GetPath();
  private readonly string inputFileName = "InputFile.txt";
  private readonly string outputFileName = "OutputFile.txt";
  private readonly string testResultFileName = "TestResult.txt";
  private string testResultPath => Path.Combine(testFilePath, "TestResult");

  [Fact]
  public async Task TestInputProcessingMatchesOutput()
  {
    string testFile = getTestFilePath(testFilePath, inputFileName);

    TemplateFileGenerator templateFileGenerator = new(testFile);
    var fileSettingsDtoList = templateFileGenerator.GetFileTemplateSettings();
    fileSettingsDtoList[0].Destination = getTestFilePath(testResultPath, string.Empty);
    fileSettingsDtoList[0].FileName = Path.GetFileNameWithoutExtension(testResultFileName);
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

      byte[] expectedOutputFile = File.ReadAllBytes(getTestFilePath(testFilePath, outputFileName));
      byte[] outputFile = File.ReadAllBytes(getTestFilePath(testResultPath, testResultFileName));

      Assert.Equal(expectedOutputFile, outputFile);
    }
    finally
    {
      File.Delete(getTestFilePath(testResultPath, testResultFileName));
    }
  }

  private static string getTestFilePath(string relativePath, string fileName)
  {
    var codeBaseUrl = new Uri(Assembly.GetExecutingAssembly().Location);
    var codeBasePath = Uri.UnescapeDataString(codeBaseUrl.AbsolutePath);
    var dirPath = Path.GetDirectoryName(codeBasePath);
    return Path.Combine(dirPath, relativePath, fileName);
  }
}
# Template Manager CLI

This tool was built to speed up the development of projects using a standard structure such as the Mediator pattern with a consistent handler/request/unit test.
The supported operating systems for this CLI is so far unknown, but it does work on windows machines.

## Running the CLI

Once this Nuget package is installed, you can run the command ```tmplt``` in most command prompts to trigger the template process.
When you run the application for the first time, it will ask you if you want the CLI to create the CodeTemplates folder for you. This will be placed at {rootdrive}/CodeTemplates by default.
In addition, it will place a json file inside of it with a link to the Example folder also in the CodeTemplates folder.
This is to enable building up a template repository from many sources.
In order to replace this functionality, you can create a "CodeTemplates" folder in any folder location, and when running ```tmplt``` in a folder navigating down from the folder containing CodeTemplates, it will search for templates from the first occurence of a CodeTemplate folder.
There is an option in the CLI that allows you to navigate to root in case you want a template not specific to a solution.

## Creating a template

Templates are just a regular file in a CodeTemplates folder that you want to create a copy of. The power comes in the templatising of variable names.
The ExampleFile contains most of the options available on a template level, there is also a section lower down explaining all of the options available.
Note that all variables are denoting with a two dollar symbols ('&#0036;').
An example is &#0036;EntityName&#0036;.
The following 2 sections will demonstrate how to create a file followed by how to create a file.

## Creating a file.

An example of this being used is as follows, below is the collectio no variables. Note that anything wrapped in &#0036;&#0036; will be read as a variable.

```
/*Usings removed for space*/
namespace &#0036;setting:namespace&#0036;;

public class &#0036;Action&#0036;&#0036;Domain&#0036; : EndpointBaseSync.WithRequest<&#0036;Action&#0036;&#0036;Domain&#0036;Request>.WithActionResult<&#0036;ReturnType&#0036;>
{
  private readonly ILogger _logger;

  public &#0036;Action&#0036;&#0036;Domain&#0036;(ILogger<&#0036;Action&#0036;&#0036;Domain&#0036;> logger)
  {
    _logger = logger;
  }

  [AllowAnonymous]
  [Http&#0036;select:RequestType:Get|Post|Delete|Patch|Put&#0036;("&#0036;Action&#0036;")]
  [SwaggerOperation(
    Summary = "&#0036;Action&#0036; &#0036;Domain:comment&#0036;",
    Description = "&#0036;Action&#0036; &#0036;Domain:comment&#0036;",
    OperationId = "&#0036;Domain&#0036;.&#0036;Action&#0036;",
    Tags = new[] { "&#0036;Domain&#0036;Endpoint" })]
  public override ActionResult<&#0036;ReturnType&#0036;> Handle([FromQuery] &#0036;Action&#0036;&#0036;Domain&#0036;Request request)
  {
    return Ok();
  }
}

public class &#0036;Action&#0036;&#0036;Domain&#0036;Request
{
}
```

{Key} is used to denote a custom variable name that will be requested during the template construction process. 

| Key | Description | Usage |
|-------|-----|------|
| &#0036;{Key}&#0036; |  left-aligned | &#0036;EntityName&#0036; |
| &#0036;{Key}:comment&#0036; | The {Key} value split with spaces | &#0036;EntityName:comment&#0036; |
| &#0036;{Key}:underscoreUppercase&#0036; | The {Key} value split with underscores and uppercased | &#0036;EntityName:comment&#0036; |
| &#0036;{Key}:underscore&#0036; | The {Key} value split with underscores with unchanges case | &#0036;EntityName:comment&#0036; |
| &#0036;{Key}:lowercase&#0036; | The {Key} value lowercased | &#0036;EntityName:comment&#0036; |
| &#0036;setting:namespace&#0036; | Uses the path from src and replaces '\\' with '.' (This hasn't been tested outside of windows) | &#0036;setting:solution&#0036; |
| &#0036;setting:projectName&#0036; | returns the name of the project name as is | &#0036;setting:projectName&#0036; |
| &#0036;setting:solution&#0036; | Returns the name of the solution by navigating to the parent until a .sln is identified | &#0036;setting:solution&#0036; |
| &#0036;setting:solutionPath&#0036; | Returns the navigation path to the solution from the user's root | &#0036;setting:solutionPath&#0036; |
| &#0036;setting:src&#0036; | Returns the path by navigating to the parent until a src folder is identified. The output is inclusive of the src folder | &#0036;setting:src |
| &#0036;if:{Key}&#0036;/&#0036;endif:{Key}&#0036; | Used to show or not show certain sections of code. Note that it takes spaces and linebreaks into account | &#0036;if:includeGet&#0036; getMethod &#0036;endif:includeGet&#0036; |
| &#0036;select:{Key}:{Option1}&#124;{Option2}&#124;{Option3}&#0036; | Used to offer a select for variables to be used. | &#0036;select:RequestType:Get&#124;Post&#124;Put&#0036; |

## Create a template group

The template groups are used to identify groupings of files that must be used together to create a greater fileset.
This is useful when you need to produce a file and an interface at the same time, or want to create your unit test at the same time as your handler.
The following example shows an API endpoint and an associated test:

Given the folder structure:

├── CodeTemplates
│   ├── ArdalisEndpoint.tmplt
│   ├── Endpoints
│   │   ├── EndpointWithRequest.cs
│   │   ├── EndpointWithRequestTest.cs

You can build a template group with the following code in the ArdalisEndpoint.tmplt:

```
{
  "ReplacementValues": [
  ],
  "Files": [
    {
      "FileName": "&#0036;Action&#0036;&#0036;Domain&#0036;.cs",
      "TemplateName": "Endpoints\\EndpointWithRequest.cs",
      "Destination": "&#0036;setting:solutionPath&#0036;\\&#0036;setting:solution&#0036;.Api\\Endpoints\\&#0036;Domain&#0036;\\",
      "Type": "File"
    },
    {
      "FileName": "&#0036;Action&#0036;&#0036;Domain&#0036;Test.cs",
      "TemplateName": "Endpoints\\EndpointWithRequestTest.cs",
      "Destination": "&#0036;setting:solutionPath&#0036;\\&#0036;setting:solution&#0036;.Api.Tests\\Endpoints\\&#0036;Domain&#0036;\\",
      "Type": "File"
    },
  ]
}
```

## Development

In order to install this package, navigate to the TemplateManager.Cli project folder and run the folliwng in a CLI:
dotnet pack
dotnet tool uninstall TemplateManager.Cli -g
dotnet tool install -g -v n --add-source ./nupkg TemplateManager.Cli

## Repository

The repository for the code maintenance can be found at:
[Template Manager on Github](https://github.com/Mossynugget/TemplateManager)
**Please note** that it is currently un-unit tested, that is my next priority. It is a fairly major concern.

## Template collection

It hasn't been built out yet, but I'm hoping to set up a template collection repository. Please feel free to contribute to it once it is set up.
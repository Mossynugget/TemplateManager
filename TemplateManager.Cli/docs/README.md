# Template Manager CLI

This tool was built to speed up the development of projects using a standard structure such as the Mediator pattern with a consistent handler/request/unit test.  
This CLI has so far been tested with full support on Windows, and partial support on Mac and Linux.

## Running the CLI

Once this Nuget package is installed, you can run the command ```tmplt``` in most command prompts to trigger the template process.  
When you run the application for the first time, it will ask you if you want the CLI to create the CodeTemplates folder for you. This will be placed at {rootdrive}/CodeTemplates by default.  
In addition, it will place a json file inside of it with a link to the Example folder also in the CodeTemplates folder.  
This is to enable building up a template repository from many sources.  
To replace this functionality, you can create a "CodeTemplates" folder in any folder location, and when running ```tmplt``` in a folder navigating down from the folder containing CodeTemplates, it will search for templates from the first occurence of a CodeTemplate folder.  
There is an option in the CLI that allows you to navigate to root in case you want a template not specific to a solution.

## Creating a template

Templates are just a regular file in a CodeTemplates folder that you want to create a copy of. The power comes in the templatising of variable names.  
The ExampleFile contains most of the options available on a template level, there is also a section lower down explaining all of the options available.  
Note that all variables are denoting with a two dollar symbols ('$').  
An example is ```$EntityName$```.  
The following 2 sections will demonstrate how to create a file followed by how to create a file.

## Recommended use case

The template manager tool has been built with the intent of making duplicate text based files with slight variances, such as varialbe names.
This has been expanded with file groupings (grouped through .tmplt files) which allows multiple files to be generated in specified locations, ideal for following strict architectures.
The regular usage is to make use of a CodeTemplates folder in the root of a project to enable all members of the team access to the templates following the code structure of the project and to enable quick editting when that structure changes.

## Creating a file.

An example of this being used is as follows, below is the collectio no variables. Note that anything wrapped in ```${variable}$``` will be read as a variable.

```
/*Usings removed for space*/
namespace $setting:namespace$;

public class $Action$$Domain$ : EndpointBaseSync.WithRequest<$Action$$Domain$Request>.WithActionResult<$ReturnType$>
{
  private readonly ILogger _logger;

  public $Action$$Domain$(ILogger<$Action$$Domain$> logger)
  {
    _logger = logger;
  }

  [AllowAnonymous]
  [Http$select:RequestType:Get|Post|Delete|Patch|Put$("$Action$")]
  [SwaggerOperation(
    Summary = "$Action$ $Domain:comment$",
    Description = "$Action$ $Domain:comment$",
    OperationId = "$Domain$.$Action$",
    Tags = new[] { "$Domain$Endpoint" })]
  public override ActionResult<$ReturnType$> Handle([FromQuery] $Action$$Domain$Request request)
  {
    return Ok();
  }
}

public class $Action$$Domain$Request
{
}
```

{Key} is used to denote a custom variable name that will be requested during the template construction process. 

| Key | Description | Usage |
|-------|-----|------|
| ```${Key}$``` |  left-aligned | ```$EntityName$``` |
| ```${Key}:comment$``` | The {Key} value split with spaces | ```$EntityName:comment$``` |
| ```${Key}:uppercaseUnderscore$``` | The {Key} value split with underscores and uppercased | ```$EntityName:uppercaseUnderscore$``` |
| ```${Key}:lowercaseDashes$``` | The {Key} value split with dashes and lowercased | ```$EntityName:lowercaseDashes``` |
| ```${Key}:underscore$``` | The {Key} value split with underscores with unchanged case | ```$EntityName:underscore$``` |
| ```${Key}:lowercase$``` | The {Key} value lower cased | ```$EntityName:lowercase$``` |
| ```${Key}:camelCase$``` | The {Key} value camel cased | ```$EntityName:camelCase``` |
| ```$setting:namespace$``` | Uses the path from src and replaces '\\' with '.' (This hasn't been tested outside of windows) | ```$setting:solution$``` |
| ```$setting:projectName$``` | returns the name of the project name as is | ```$setting:projectName$``` |
| ```$setting:solution$``` | Returns the name of the solution by navigating to the parent until a .sln is identified (Currently very C# specific) | ```$setting:solution$``` |
| ```$setting:destination$``` | Returns the executed path from the terminal | ```$setting:solution$``` |
| ```$setting:solutionPath$``` | Returns the navigation path to the solution from the user's root | ```$setting:destination$``` |
| ```$setting:src$``` | Returns the path by navigating to the parent until a src folder is identified. The output is inclusive of the src folder | ```$setting:src$``` |
| ```$if:{Key}$/$endif:{Key}$``` | Used to show or not show certain sections of code. Note that it takes spaces and linebreaks into account | ```$if:includeGet$ getMethod $endif:includeGet$``` |
| ```$else:{Key}$/$endelse:{Key}$``` | Used in the negative of the if. Else can not exist without the associated if. | ```$else:includeGet$ getMethod $endelse:includeGet$``` |
| ```$select:{Key}:{Option1}|{Option2}|{Option3}$``` | Used to offer a select for variables to be used. | ```$select:RequestType:Get|Post|Put$``` |

In situtations where you need a variable followed by text followed by another variabels (i.e. $if:IncludeX$X$endif:IncludeX$) - Where x is a specific variable -, you can use _$_ to break the search chain (i.e. $if:IncludeX$_$_X$endif:IncludeX$). The _$_ will be deleted afterwards.

## Create a template group

The template groups are used to identify groupings of files that must be used together to create a greater fileset.  
This is useful when you need to produce a file and an interface at the same time, or want to create your unit test at the same time as your handler.  
The following example shows an API endpoint and an associated test:  

Given the folder structure:

```
├── CodeTemplates
│   ├── ArdalisEndpoint.tmplt
│   ├── Endpoints
│   │   ├── EndpointWithRequest.cs
│   │   ├── EndpointWithRequestTest.cs
```

You can build a template group with the following code in the ArdalisEndpoint.tmplt:

```
{
  "ReplacementValues": [
  ],
  "Files": [
    {
      "FileName": "$Action$$Domain$.cs",
      "TemplateName": "Endpoints\\EndpointWithRequest.cs",
      "Destination": "$setting:solutionPath$\$setting:solution$.Api\\Endpoints\$Domain$\\",
      "FileType": "File"
    },
    {
      "FileName": "$Action$$Domain$Test.cs",
      "TemplateName": "Endpoints\\EndpointWithRequestTest.cs",
      "Destination": "$setting:solutionPath$\$setting:solution$.Api.Tests\\Endpoints\$Domain$\\",
      "FileType": "File"
    },
    {
      "FileName": "Startup.cs",
      "TemplateName": "Endpoints\\RepositoryDI.cs",
      "Destination": "$setting:solutionPath$\$setting:solution$.Api\\",
      "LineIdentifier": "// Repository DI here"
      "FileType": "Snippet"
    },
  ]
}
```

## Template Group File Types

There are currently 2 options for file types.
The default is ```File```, which will create a new file.
The second option is the Snippet type which will insert the contents of the template into the line under the "LineIdentifier."

## Development

In order to install this package, navigate to the TemplateManager.Cli project folder and run the folliwng in a CLI:  
So from base you will run (for Windows):

```
cd .\TemplateManager.Cli\
dotnet pack
dotnet tool uninstall TemplateManager.Cli -g
dotnet tool install -g -v n --add-source ./nupkg TemplateManager.Cli
```

## Repository

The repository for the code maintenance can be found at:  
[Template Manager on Github](https://github.com/Mossynugget/TemplateManager)  

## Template collection

There is an early development of a [Template Library](https://github.com/Mossynugget/TemplateManager/tree/main/TemplateLibrary). To utilize them, please copy them out and edit them to your use within your own projects.  
Please feel free to add new or edit edit existing templates to match a more global standard.  
All contributions to the project will be greatly appreciated.
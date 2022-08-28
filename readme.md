In order to install this package, navigate to the TemplateManager.Cli project folder and run the folliwng in a PowerShell:
dotnet pack
dotnet tool uninstall TemplateManager.Cli -g
dotnet tool install -g -v n --add-source ./nupkg TemplateManager.Cli


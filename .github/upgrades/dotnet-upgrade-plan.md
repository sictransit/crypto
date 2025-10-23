# .NET 8.0 Upgrade Plan

## Execution Steps

Execute steps below sequentially one by one in the order they are listed.

1. Validate that an .NET 8.0 SDK required for this upgrade is installed on the machine and if not, help to get it installed.
2. Ensure that the SDK version specified in global.json files is compatible with the .NET 8.0 upgrade.
3. Upgrade Common\Common.csproj
4. Upgrade src\Enigma\Enigma.csproj
5. Upgrade Axiom\Axiom.csproj
6. Upgrade src\EnigmaConsole\EnigmaConsole.csproj
7. Upgrade tests\UnitTests.csproj
8. Run unit tests to validate upgrade in the projects listed below:
  tests\UnitTests.csproj

## Settings

This section contains settings and data used by execution steps.

### Excluded projects

Table below contains projects that do belong to the dependency graph for selected projects and should not be included in the upgrade.

| Project name                                   | Description                 |
|:-----------------------------------------------|:---------------------------:|

### Aggregate NuGet packages modifications across all projects

(No NuGet package modifications were identified by analysis.)

### Project upgrade details
This section contains details about each project upgrade and modifications that need to be done in the project.

#### Common\\Common.csproj modifications

Project properties changes:
  - Target framework should be changed from `net5.0` to `net8.0`

#### src\\Enigma\\Enigma.csproj modifications

Project properties changes:
  - Target framework should be changed from `net5.0` to `net8.0`

#### Axiom\\Axiom.csproj modifications

Project properties changes:
  - Target framework should be changed from `net5.0` to `net8.0`

#### src\\EnigmaConsole\\EnigmaConsole.csproj modifications

Project properties changes:
  - Target framework should be changed from `net5.0` to `net8.0`

#### tests\\UnitTests.csproj modifications

Project properties changes:
  - Target framework should be changed from `net5.0` to `net8.0`

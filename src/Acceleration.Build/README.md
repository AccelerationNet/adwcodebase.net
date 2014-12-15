\namespace Acceleration.Build
\brief Helpers for MSBuild.

### Versioning

Add git information to assemblies.

 1. install git to `C:\Program Files (x86)\Git\bin\git.exe`
 1. nuget install `Acceleration.Build` in your project
 1. add `**/version.generated.cs` to your `.gitignore`
 1. be sure there's no `AssemblyInformationalVersion` specified elsewhere


### Common

Various tasks and analyses targets meant to be used from MSBuild
directly for either publishing or continuous integration.

To install:

 * make a `.proj` file at the root of your project (in the same folder as `src`)
 * copy `Acceleration.proj` as a template
 * change the import path to match
 * change the property configuration to match your project
 * In visual studio, install nuget packages `NUnit.Runners` and
   `Mono.Gendarme` - these copy in binaries for these tools


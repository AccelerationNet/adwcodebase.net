\namespace Acceleration.Build
\brief Helpers for MSBuild.

### Versioning

Add git information to assemblies.

To install in a csproj:

 1. open the csprof and add:

        <Import Project="../../../adwcodebase.net/src/Build/tools/Versioning.targets" />
		
 1. add `**/version.generated.cs` to your `.gitignore`
 1. be sure there's no `AssemblyInformationalVersion` specified elsewhere
 1. reference the `Acceleration.Build.csproj`
 1. use `Acceleration.Build.Versioning.VersionInfo` to read version
    information about an assembly

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


# Acceleration.Build

Helpers for MSBuild.

### Versioning

Add git information to assemblies.

To install in a csproj:

 1. open the csprof and add:

        <Import Project="../../../adwcodebase.net/src/Acceleration.Build/tools/Versioning.targets" />
		
 1. add `**/version.generated.cs` to your `.gitignore`
 1. be sure there's no `AssemblyInformationalVersion` specified elsewhere
 1. reference the `Acceleration.Build.csproj`
 1. use `Acceleration.Build.Versioning.VersionInfo` to read version
    information about an assembly

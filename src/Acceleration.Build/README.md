# Acceleration.Build

Helpers for MSBuild.

### Versioning

Add git information to assemblies.

To install in a csproj:

 1. open the csprof and add:

        <Import Project="..\path\to\Versioning.targets" />
		
 1. add `**/version.generated.cs` to your `.gitignore`
 1. be sure there's no `AssemblyInformationalVersion` specified elsewhere

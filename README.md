# adwcodebase.net

Shared codebase for .NET projects.

 * root namespace: `Acceleration`
 * contains shared msbuild files
 * multiple assemblies for different extensions, minimizing
   dependencies
    * `Acceleration` - base extensions to framework things like
      `ICollection`, `Linq`, etc
	* assembly for each library: `Acceleration.MVC`,
      `Acceleration.Ninject`, `Acceleration.EF`
 * all extension methods go into the `Acceleration.Extensions`
   regardless of assembly. Buys convenience for the risk of
   compile-time name conflicts, which are easily resolvable.
 * use nuget conventions to enable nuget deployment later
 * checkout as siblings to other projects, use `../../adwcodebase.net/src/foo/foo.csproj` to
   reference them 

## How to use

 * checkout `adwcodebase.net` in the same folder with working copies
   of your other .NET projects
    
	  git clone gitolite@git.acceleration.net:adwcodebase.net.git
	  
 * open your other .NET project's `Sln` file in visual studio
 * File -> Add Existing Project
 * Navigate up to your working copies folder, then into your
   `adwcodebase.net` checkout and pick the `csproj` file for the
   library you want to use
   
## Future

If these libraries stabilize, then we can publish them to nuget/github
and use stock nuget for easier inclusion in .NET projects
    
## Contents

See each project's readme for more information.

 * `Acceleration` - base classes and extensions to fundamental .NET
   framework classes
 * `Acceleration.Build` - MSBuild helpers

## Guidelines

 * make extension methods when it feels like you're adding missing
   functionality; a lot of the time it makes more sense to just use a
   static method.
 * put extension methods in the `Acceleration.Extensions`

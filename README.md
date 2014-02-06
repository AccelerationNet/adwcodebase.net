adwcodebase.net {#mainpage}
===============

Shared codebase for .NET projects.

## Building 

We have MSBuild targets setup:

 * `/t:Build` - builds everything
 * `/t:Test` - builds everything and runs the tests
 * `/t:Analyze` - builds everything and runs static analysis
 * `/t:Jenkins` - builds everything, runs tests, runs static analysis
 * `/t:Docs` - make the docs

## How to use

We publish packages to nuget, install from there:

* Acceleration.MappedReader
* Acceleration.Collections

For the rest:

 * checkout `adwcodebase.net` in the same folder with working copies
   of your other .NET projects
    
	    git clone gitolite@git.acceleration.net:adwcodebase.net.git
	  
 * open your other .NET project's `Sln` file in visual studio
 * File -> Add Existing Project
 * Navigate up to your working copies folder, then into your
   `adwcodebase.net` checkout and pick the `csproj` file for the
   library you want to use
   
    
## Contents

See each project's readme for more information.

 * `Acceleration.Core` - base classes and extensions to fundamental .NET
   framework classes
 * `Acceleration.Build` - MSBuild helpers
 * `Acceleration.Testing` - common testing setup for `NUnit` and `MSTest`
 * `Tests` - unit tests for the shared code

## Guidelines

 * make extension methods when it feels like you're adding missing
   functionality; a lot of the time it makes more sense to just use a
   static method.
 * put a `README.md` in each project
 * write unit tests

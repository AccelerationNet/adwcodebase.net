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
* `/t:NugetPackage /p:LibToPackage=X` - make a nuget package ready to
   be uploaded
 
## How to use

We publish packages to nuget, install from there:

* `Acceleration.MappedReader`
* `Acceleration.Collections`
* `Acceleration.Disposable`
* `Acceleration.Versioning`

See each project's readme for more information.

## Guidelines

 * make extension methods when it feels like you're adding missing
   functionality; a lot of the time it makes more sense to just use a
   static method.
 * put a `README.md` in each project
 * write unit tests

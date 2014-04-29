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
* `/t:package /p:LibToPackage=X` - make a nuget package ready to
   be uploaded
* `/t:push /p:LibToPackage=X` - upload a nuget package, depends on
  having your workstation configured

## How to use

We publish packages to nuget, install from there:

* `Acceleration.MappedReader`
* `Acceleration.Collections`
* `Acceleration.Disposable`
* `Acceleration.Versioning`
* `Acceleration.Hmac`

See each project's readme for more information.

## Guidelines

* make extension methods when it feels like you're adding missing
  functionality; a lot of the time it makes more sense to just use a
  static method.
* put a `README.md` in each project
* write unit tests, with a unit test project for each library; we
  might split this into separate git repos later
* we mirror this on github, so nothing sensitive!

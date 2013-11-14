# Acceleration.NUnit

NUnit helpers to handle common needs:

 * logging
 * running a function once during a test run
 
## Usage

 * import the library into your unit testing lib
 * make a `Base` test class, inherit from `Acceleration.NUnit.Base`
 * for one-time setup, override `OneTimeFixtureSetup` - will only run
   once per nunit execution

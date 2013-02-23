# What's new #
- When you're looking for a member in a hierarchy or level and this member is not found, NBi will display members of this hierarchy or level (Max 10).
- New config file replacing the previous plain text file to redirect to the test-suite **[Breaking change]**
- Support of AutoCategory, to add automatically categories based on the tests written (enabled by config file)
- Support connectionStrings from the config file
- When comparing two result-sets and one of them has a violation of the unique key defined, NBi  will display the rows violating this unique key.
- Path for dependencies (sql, mdx or csv files) are now relative to nbits files and not anymore to AppBase (generally NBi.NUnit.Runtime.dll) **[Breaking change]**
- Comprehensive message when a dependency (sql, mdx or csv files) is not found. When a dependency is not found, NBi displays the expected full-path of this dependency.
- You can now specify the reason of the ignore attribute on a test
- Exists constraint for a "Measure" takes into account the display-folder and measure-group 
- When a test is failing, NBi will display the test (xml format) and not anymore the C# stacktrace
- Support of test's name dependant of test's definition
- Better Expectation and Actual messages for Structure constraints
- Added a tag 'edition' to mamange change in test-suites
- Tutorial on how to share the binaries (NBi.Core.Dll, NBi.Xml.Dll, ...) with Gallio and NUnit

# What's fixed #
- Support CSV files with zero row
- When column's name was too short, NBi was crashing (Fixed by Cédric Bourgeois)
- Fixed a bug with default connectionString when using the constraint 'Contains' with a structure for system-under-test

# What's cool #
- First commits made by someone else than the project's owner
- Source code is (finally) correctly configured to support NuGet and SharpDevelop
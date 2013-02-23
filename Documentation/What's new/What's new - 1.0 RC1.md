# What's new? #
## Enhancements and new features ##
- Introduction of an assert named *exists* to facilitate *structure* and *members* tests
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
- Support of test's name dependent of test's definition
- Better Expectation and Actual messages for Structure constraints
- Added a tag 'edition' to manage change in test-suites
- Tutorial on how to share the binaries (NBi.Core.Dll, NBi.Xml.Dll, ...) with Gallio and NUnit

## Breaking changes ##
We've updated the XSD defining the structure of the xml tests and introduced some breaking changes. We're confident that it's a change for good and sure that you'll be able to quickly update your existing test-suite to be compatible with this new release.

- In system-under-test, the xml elements *structure* and *members* don't define anymore on what you'll perform your test. This definition is managed in oen of the following child elements: perspective, dimension, hierarchy, level, measure-group, measure 
- In system-under-test, the xml element *query* must now be integrated inside a parent element named *execution*
We're confident that these changes will be positive for the future of NBi

# What's fixed? #
- Support CSV files with zero row
- When column's name was too short, NBi was crashing (Fixed by Cédric Bourgeois)
- Fixed a bug with default connectionString when using the constraint 'Contains' with a structure for system-under-test

# What's cool? #
- First commits made by someone else than the project's owner
- Source code is (finally) correctly configured to support NuGet and SharpDevelop
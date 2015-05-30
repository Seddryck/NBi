This version is a the final release of version 1.1 and includes following features:
! What's new?
!! Enhancements and new features
* Test availability of one or more elements in the cube structure
* Test the relation between a dimension and a measure-group
* Test availability of one or more members in a hierarchy or level
* Manage connection strings (from config file)
* Get a query from an assembly _(Documentation will be provided soon)_
* Several enhancements to facilitate the diagnostic of common issues with NBi files (missing connection, csv files with headers, incorrect formats for columns, ...)
*since v1.1RC*
* All features are documented
* Timeout option for the test _fasterThan_
* Settings' management in Genbi
* Progress bar when generating tests in Genbi
* feature to remove a column/variable from the CSV in Genbi

!! Breaking changes
* The test previously named "contains" is renamed into "contain".

! What's fixed?
* No bug has been reported on 1.0
*Fixed since the release-candidate v1.1RC*
* Fixed error message when a LinkedTo is failing
* Fixed numerous display issues with Genbi (Genbi)
* Fixed crash when the resulting test was incorrect from an xml point-of-view (Genbi)
* _Settings_ element is was at the bottom of the test-suite (Genbi)
* Some values for the attributes of the test _EqualTo_ were automatically set by Genbi even in the case they are the default value (Genbi)
* Edition tag is now saved when generating a test suite (Genbi)

! What's cool?
* NBi is now including a *tests generator*. Based on csv files and templates, you can quickly create large tests suites.
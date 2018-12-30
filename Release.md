# Release 1.19-RC1

This release 1.19 extends the work on the support of data quality checks. This new release introduces the support of tests around referential integrity. You can now assert if a result-set has all its foreign-keys pointing to a reference result-set (or the other way). Also interesting to note that we're expanding the usage of variables, they are now usable in more places and can be overridden in the configuration file. Another new feature, to be develop further in the next releases, is the scoring of a result-set. This feature lets you define a percentage of quality, clearly stating if a result-set is fit for purpose or not, in place of reporting a number of rows violating a predicate.

## What's new

### Framework

- [#352](https://github.com/Seddryck/NBi/issues/352) - Add support for checking the referencial integrity of a table when defined on one or more columns
- [#413](https://github.com/Seddryck/NBi/issues/413) - Support reversed lookup
- [#406](https://github.com/Seddryck/NBi/issues/406) - Basic support of scoring for a result-set
- [#414](https://github.com/Seddryck/NBi/issues/414) - Full support of variables for predicate’s reference
- [#421](https://github.com/Seddryck/NBi/issues/421) - Add support for dynamic filename in result-set
- [#419](https://github.com/Seddryck/NBi/issues/419) - You can now reference a variable as a parameter of a query in the assert block
- [#410](https://github.com/Seddryck/NBi/issues/410) - Read environment variables in config/test suite
- [#409](https://github.com/Seddryck/NBi/issues/409) - Override variables' value within the config file
- [#363](https://github.com/Seddryck/NBi/issues/363) - New filter to reduce a result-set by selecting first/last rows
- [#415](https://github.com/Seddryck/NBi/issues/415) - Support a tolerance 'ignore-case' when comparing two textual values
- [#402](https://github.com/Seddryck/NBi/issues/402) - Add some native transformations for dateTime
- [#394](https://github.com/Seddryck/NBi/issues/394) - Support for Data Source using asazure protocol 
- [#404](https://github.com/Seddryck/NBi/issues/404) - Facilitate the embeding of NBi into an application or service
- [#365](https://github.com/Seddryck/NBi/issues/365) - *(bug fix)* any-of's reference is converted from a list of string to a single string
- [#416](https://github.com/Seddryck/NBi/issues/416) - *(bug fix)* Stop to throw a timeout error after 0 seconds message when the underlying reason is a syntax error in the query (was only the case for SSAS)
- [#423](https://github.com/Seddryck/NBi/issues/423) - Minor improvement when dealing with errors and predicate's operand
- [#374](https://github.com/Seddryck/NBi/issues/374) - *(bug fix)* Multiple expressions don't work with a 'all-rows' constraint
- [#373](https://github.com/Seddryck/NBi/issues/373) - Improve debugging experience for 'all-rows' constraints with an expression

### genbiL

- [#375](https://github.com/Seddryck/NBi/issues/375) - Trim columns' content in genbiL
- [#389](https://github.com/Seddryck/NBi/issues/389) - ```case cross``` is now supporting a join on multiple columns
- [#401](https://github.com/Seddryck/NBi/issues/401) - *(bug fix)* genbiL adding attributes ```column-index``` and ```type``` to ```expression``` element
- [#392](https://github.com/Seddryck/NBi/issues/392) - *(bug fix)* genbiL adding column-index to the ```transformation``` element 
- [#376](https://github.com/Seddryck/NBi/issues/376) - *(bug fix)* Action ```cross``` doesn't work when one the columns contains an array
- [#372](https://github.com/Seddryck/NBi/issues/372) - *(bug fix)* Serialization of expression includes unexpected attributes
- [#422](https://github.com/Seddryck/NBi/issues/422) - *(bug fix)* Incorrect serialization for csv-profile (missing-cell and empty-cell)
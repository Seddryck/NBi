![Logo](https://github.com/Seddryck/nbi/raw/gh-pages/img/logo-2x.png)
# NBi #
NBi is a **testing framework** (add-on to NUnit) for **Business Intelligence**. It supports most of the relational databases (SQL server, MySQL, postgreSQL ...) and OLAP platforms (Analysis Services, Mondrian ...) but also ETL and reporting components (Microsoft technologies).

The main goal of this framework is to let users create tests with a declarative approach based on an **Xml** syntax. By the means of NBi, you don't need to develop C# code to specify your tests! Either, you don't need Visual Studio to compile your test suite. Just create an Xml file and let the framework interpret it and play your tests. The framework is designed as an add-on of NUnit but with the possibility to port it easily to other testing frameworks.

**Social media:** [![website](https://img.shields.io/badge/website-nbi.io-fe762d.svg)](http://www.nbi.io)
[![twitter badge](https://img.shields.io/badge/twitter-@Seddryck-blue.svg?style=flat&logo=twitter)](https://twitter.com/Seddryck)

**Releases:** [![nuget](https://img.shields.io/nuget/v/NBi.Framework.svg)](https://www.nuget.org/packages/NBi.Framework/)
[![GitHub Release Date](https://img.shields.io/github/release-date/seddryck/nbi.svg)](https://github.com/Seddryck/NBi/releases/latest)
[![licence badge](https://img.shields.io/badge/License-Apache%202.0-yellow.svg)](https://github.com/Seddryck/NBi/blob/master/LICENSE)
[![FOSSA Status](https://app.fossa.com/api/projects/git%2Bgithub.com%2FSeddryck%2FNBi.svg?type=shield)](https://app.fossa.com/projects/git%2Bgithub.com%2FSeddryck%2FNBi?ref=badge_shield)

**Latest RC and beta:** [![Pre-release](https://img.shields.io/github/tag-pre/seddryck/nbi.svg?color=%23ee41f4&label=Pre-release)](https://github.com/Seddryck/NBi/releases/)
![GitHub (Pre-)Release Date](https://img.shields.io/github/release-date-pre/Seddryck/NBi?label=Pre-release)
[![nuget](https://img.shields.io/nuget/vpre/NBi.Framework.svg?color=%23427682&label=Beta)](https://www.nuget.org/packages/NBi.Framework/)

**Dev. activity:** [![GitHub last commit](https://img.shields.io/github/last-commit/Seddryck/nbi.svg)](https://github.com/Seddryck/NBi/releases/latest)
![Still maintained](https://img.shields.io/maintenance/yes/2024.svg)
![GitHub commits since latest version](https://img.shields.io/github/commits-since/Seddryck/NBi/latest/develop)
![GitHub commits on v2.0](https://img.shields.io/github/commits-since/seddryck/nbi/v1.21/develop_v2?label=commits%20on%20v2.0)
![GitHub commit activity](https://img.shields.io/github/commit-activity/y/Seddryck/NBi)

**Continuous integration builds:** [![Build status](https://ci.appveyor.com/api/projects/status/t5m0hr57vnsdv0v7?svg=true)](https://ci.appveyor.com/project/Seddryck/nbi)
[![Tests](https://img.shields.io/appveyor/tests/seddryck/nbi.svg)](https://ci.appveyor.com/project/Seddryck/nbi/build/tests)
[![CodeFactor](https://www.codefactor.io/repository/github/seddryck/nbi/badge)](https://www.codefactor.io/repository/github/seddryck/nbi)

**Status:** [![stars badge](https://img.shields.io/github/stars/Seddryck/NBi.svg)](https://github.com/Seddryck/NBi/stargazers)
[![Bugs badge](https://img.shields.io/github/issues/Seddryck/NBi/bug.svg?color=red&label=Bugs)](https://github.com/Seddryck/NBi/issues?utf8=%E2%9C%93&q=is:issue+is:open+label:bug+)
[![Features badge](https://img.shields.io/github/issues/seddryck/nbi/feature-request.svg?color=purple&label=Feature%20requests)](https://github.com/Seddryck/NBi/issues?utf8=%E2%9C%93&q=is:issue+is:open+label:feature-request+)
[![Top language](https://img.shields.io/github/languages/top/seddryck/nbi.svg)](https://github.com/Seddryck/NBi/search?l=C%23)

## Releases ##
Binaries for the different releases are hosted on [www.nbi.io](http://www.nbi.io/release/) or [GitHub](https://github.com/Seddryck/NBi/releases)

## Documentation ##
The documentation is available on-line and is hosted on [www.nbi.io](http://www.nbi.io/docs/home/)

## Licenses ##
NBi is available on the terms of Apache 2.0. NBi is also using several OSS projects as libraries. 

Compatibility of licenses in checked by FOSSA app:

[![FOSSA Status](https://app.fossa.com/api/projects/git%2Bgithub.com%2FSeddryck%2FNBi.svg?type=large)](https://app.fossa.com/projects/git%2Bgithub.com%2FSeddryck%2FNBi?ref=badge_large)

## Bugs, issues and requests for features ##
The list of bugs and feature's requests is hosted on [GitHub](https://github.com/Seddryck/NBi/issues)

## Continuous Integration and Testing ##
A continuous integration service is available on [AppVeyor](https://ci.appveyor.com/project/Seddryck/nbi) and another on [Azure DevOps](https://seddryck.visualstudio.com/NBi/_build)

NBi has around 2750 automated tests, asserting most of the features supported by NBi during the build processes. These tests are organized in three folders:

- Acceptance: The tests are effectively written in nbits file and played end-to-end by the framework itself. They don't use any fake, mock or stub and are connected to real databases and cubes and perform queries on them.
- Integration: These tests are used to assert interactions with external resources such as databases or cubes. They make usage of stubs to define parameters impacting the code to use.
- Unit: These tests are never contacting an external resource and have a maximal scope equivalent to the code of a single class. Usage of stubs, fakes and mocks is welcome.

In order to be able to build the software on different machines, the database and cube used during tests must always be Adventure Works 2008R2. In order to facilitate the integration, NBi is connected by default to the online SQL database hosted on Azure (Unfortunately no equivalent for SSAS). If you want to override the connection settings to execut the tests on your own environment, create a file named *ConnectionString.user.config* in the folder *NBi.Testing* and copy the content from the file ConnectionString.config into it, before adjusting for your environment.

Note that all the tests are not executed on the continuous integration services, due to limitations in the availability of some components.

- Unit tests are always executed
- Integration tests are executed based on the availability of the underlying components:
    - Database Engine: Yes. Due to the usage of an Azure database to run these tests, these tests are enabled on the CI platforms. About ODBC drivers, the *ODBC driver for SQL Server (13.1)* is used on appVeyor and the *SQL Server (10.0)* is used on Azure DevOps
    - OLAP Engine: No
    - ETL Engine (SSIS): No
    - Windows Service: No (but planned to integrate them)
    - Local Database: No (but planned to integrate them)
    - Report Server: No (but planned to integrate them)
- Acceptance tests are partially run. The test-suites covering the acceptance tests are executed but will return an *ignore* result when at least one of the test is not runnable due to service not running (most of the time, the reason for an ignore is the unavailability of Analysis Services).

Three artefacts are packaged and published by this CI:

- Framework.zip contains the dll needed to run tests written with NBi
- UI.zip contains the exe and dlls needed to run Genbi
- Nuget packages

The nuget packages built on appVeyor are pushed to nuget for beta, release candidates and release branches.

## Code and contributions ##
NBi is using **Git** as DCVS and the code is hosted on [Github](https://github.com/Seddryck/NBi). Organization of the Git repository is based on [Git-flow](https://danielkummer.github.io/git-flow-cheatsheet/). 

If you want to develop a new feature, you're encouraged to read the [contribution guidelines](https://github.com/Seddryck/NBi/blob/develop/contributing.md).

NBi is mostly developed in [C#](https://github.com/Seddryck/NBi/search?l=c%23) with a bit of [SQL](https://github.com/Seddryck/NBi/search?l=sql) and [XML](https://github.com/Seddryck/NBi/search?l=xml).

## Tracking ##
This OSS project is tracked by [Ohloh](http://www.ohloh.net/p/NBi)

[![Project Stats](https://www.openhub.net/p/nbi/widgets/project_thin_badge.gif)](https://www.openhub.net/p/nbi)

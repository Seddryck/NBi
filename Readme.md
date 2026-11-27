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
![Still maintained](https://img.shields.io/maintenance/yes/2025.svg)
![GitHub commit activity](https://img.shields.io/github/commit-activity/y/Seddryck/NBi)

**Continuous integration builds:** [![.NET 10 CI](https://github.com/Seddryck/NBi/actions/workflows/ci.yml/badge.svg)](https://github.com/Seddryck/NBi/actions/workflows/ci.yml)

**Status:** [![stars badge](https://img.shields.io/github/stars/Seddryck/NBi.svg)](https://github.com/Seddryck/NBi/stargazers)
[![Bugs badge](https://img.shields.io/github/issues/Seddryck/NBi/bug.svg?color=red&label=Bugs)](https://github.com/Seddryck/NBi/issues?utf8=%E2%9C%93&q=is:issue+is:open+label:bug+)
[![Features badge](https://img.shields.io/github/issues/seddryck/nbi/feature-request.svg?color=purple&label=Feature%20requests)](https://github.com/Seddryck/NBi/issues?utf8=%E2%9C%93&q=is:issue+is:open+label:feature-request+)
[![Top language](https://img.shields.io/github/languages/top/seddryck/nbi.svg)](https://github.com/Seddryck/NBi/search?l=C%23)

## Releases ##
Binaries for the different releases are hosted on [www.nbi.io](http://www.nbi.io/release/) or [GitHub](https://github.com/Seddryck/NBi/releases)

## Documentation ##
To run your first NBi suite, follow the [test environment setup](https://www.nbi.io/docs/installation-environment/), create an [XML test suite](https://www.nbi.io/docs/installation-test-suite/), and set up the [NUnit project](https://www.nbi.io/docs/installation-nunit-project/) and its [configuration](https://www.nbi.io/docs/installation-config/). Then [run the suite with NUnit](https://www.nbi.io/docs/run-with-nunit/). Browse the [full documentation](https://www.nbi.io/docs/home/) for more examples.

## Licenses ##
NBi is available on the terms of Apache 2.0. NBi is also using several OSS projects as libraries. 

Compatibility of licenses in checked by FOSSA app:

[![FOSSA Status](https://app.fossa.com/api/projects/git%2Bgithub.com%2FSeddryck%2FNBi.svg?type=large)](https://app.fossa.com/projects/git%2Bgithub.com%2FSeddryck%2FNBi?ref=badge_large)

## Bugs, issues and requests for features ##
The list of bugs and feature's requests is hosted on [GitHub](https://github.com/Seddryck/NBi/issues)

## Continuous Integration and Testing ##
[GitHub Actions](https://github.com/Seddryck/NBi/actions/workflows/ci.yml) builds the .NET 10 solution on Windows and runs the Core, GenbiL, XML, and Framework tests. The workflow uploads test results, coverage, and the NBi, Extensibility, and Framework NuGet packages.

After a successful build on `main`, GitHub Actions publishes to NuGet.org when the `NUGET_USER` repository variable is set. Set it to the NuGet.org account name and configure a [Trusted Publishing policy](https://learn.microsoft.com/en-us/nuget/nuget-org/trusted-publishing) for this repository and `ci.yml`. The workflow uses the policy to request a short-lived publishing credential.

The legacy acceptance and integration tests are under `NBi.Testing`. To supply local database connection settings, copy `ConnectionString.config` to `ConnectionString.user.config` in that directory and adjust it for your environment.

## Code and contributions ##
See the [contribution guide](contributing.md) for reporting issues and submitting changes.

NBi is mostly developed in [C#](https://github.com/Seddryck/NBi/search?l=c%23) with a bit of [SQL](https://github.com/Seddryck/NBi/search?l=sql) and [XML](https://github.com/Seddryck/NBi/search?l=xml).

## Tracking ##
This OSS project is tracked by [Ohloh](http://www.ohloh.net/p/NBi)

[![Project Stats](https://www.openhub.net/p/nbi/widgets/project_thin_badge.gif)](https://www.openhub.net/p/nbi)

---
layout: documentation
title: NBi's setup
prev_section: home
next_section: installation-environment
permalink: /docs/installation-tools/
---
## Pre-requisistes

First, you must download and install the prerequisites:

* .Net Framework 4.5 (or higher) from the [MSDN download page](http://www.microsoft.com/en-us/download/details.aspx?id=30653)
* NUnit 2.5 (or higher) – [NUnit.org download page](http://www.nunit.org/index.php?p=download)
* Microsoft Analysis Services Adomd Client (ADOMD.NET) 10 – [MSDN download page](http://www.microsoft.com/fr-fr/download/details.aspx?id=30440), note that for Windows 64-bits you will need SQLSERVER2008_ASADOMD10_amd64.msi, for Windows 32-bits you will need the file SQLSERVER2008_ASADOMD10_x86.msi

The latest component, ADOMD.NET, is only required if you want to query an OLAP cube. The expected version is the version 10, originally shipped with SQL Server 2008R2, but freely available on the MSDN download page. NBi will specifically look for the version 10, so if your computer host a version 11 (SQL Server 2012) or 12 (SQL Server 2014), it won't be enough to run tests on an SSAS instance. Nevertheless, with version 10, you can run tests on a SQL Server 2012 or SQL Server 2014 without issues. More, you can install multiple versions of ADOMD.NET on a single server/computer and benefit from their peaceful coexistence (side-by-side versioning).

## NBi

Then, download the [latest version of NBi]({{ site.repository }}/releases), save the zip file. We'll explain in the next [chapter](../installation-test-suite) where to copy the content of this zip file.
---
layout: documentation
title: Alternative setups
prev_section: run-with-nunit
next_section: equivalence-resultsets
permalink: /docs/run-alterative/
---
The installation described in the previous pages is probably the more common and helpful but it's possible to setup NBi and the test environments differently depending of your needs. Here under, you'll find more information about running tests with *Gallio Icarius* and usage of a shortcut for quick test-suite.

## Gallio Icarius

Gallio Icarius is a tests-runner initially developed by the team building MbUnit (.Net Unit testing framework). This runner supports a lot of Unit framework including NUnit. This project is deprecated but the runner has some fans and it's possible to run test-suites built with NBi on base of this runner.

Gallio is more strict than NUnit on some points and particulary it's requiring that the config file is at in the root folder (and not in a sub-folder). To design a valid test-environment, just move the config file to the root folder. More the name of the config file should absolutely be **NBi.NUnit.Runtime.dll.config** (yes, with a .dll.config).

Start Gallio Icarius and choose the menu option "file > new project". Then click on "+ Add file" button and select the dll named NBi.NUnit.Runtime.dll (available in your installation folder). Gallio will load your test-suite.

![Gallio Icarius](../../img/docs/run-alternative/gallio-run.png)

## Unique nbits file

Sometimes, it's useful to create a quick test-suite without config or project files. In this case, the quickest way to create a test-suite is to add a file with the extension *.nbits* in the directory with NBi framework. Then start NUnit GUI (or Gallio) and select the assembly *nbi.runtime.dll*. This dll will look in its folder after *.nbits* files and load the first file found.

An alternative is to create a file named *NBi.NUnit.Runtime.dll.config (with "dll"!)*. In this file, just add the standard configuration of an NBi test-suite including the path to your test suite. In the sample bellow, the test suite is defined in the sub-directory named "SubDirectory" in file "myTestSuite.nbits". Then start NUnit GUI (or Gallio) and select the assembly *nbi.runtime.dll*. This dll will look in its folder after its configuration file *NBi.NUnit.Runtime.dll.config* files then read it and redirect to the test-suite specified in this file.

{% highlight xml %}
<?xml version="1.0" encoding="utf-8" ?>
<configuration>
	<configSections>
		<section name="nbi" type="NBi.NUnit.Runtime.NBiSection, NBi.NUnit.Runtime"/>
	</configSections>
	<nbi testSuite="SubDirectory\myTestSuite.nbits"/>
</configuration>
{% endhighlight %}

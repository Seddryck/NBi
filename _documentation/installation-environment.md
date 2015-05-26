---
layout: documentation
title: Test environment's setup
prev_section: installation-tools
next_section: installation-test-suite
permalink: /docs/installation-environment/
---
## Folders and files
A test environment is built upon the following directories hierarchy:

the base directory, in the example: *…\MyTests\* that will include:

* a directory for the NBi framework (and other non-mandatory NBi stuff present in the zip file)
* a directory for each test subfolder (depending on how you organize your test projects, start with one)

To illustrate that, a folder *MyTests* has been created, with the sub-folders *Framework*, *Genbi* and *Samples* coming from the NBi zip file previously downloaded. There is also a sub-folder *MyTestProject* that I created myself:

![repository structure](../../img/docs/installation-test-environment/intro_01.png)

Then the minimal setup to start using NBi is to create 3 files. They are text files using an XML syntax (meaning that NotePad, NotePad++, Sublime Text, Visual Studio or any other text file editor are able to edit them) with the following naming convention (note the file extensions in each area):

![repository structure](../../img/docs/installation-test-environment/intro_02.png)

## Content of the files

* The .nbits file: the actual test-suite where all your tests live. [How to build it](../installation-test-suite)
* The .config file: It references the test-suite's file.  You need to [initialize it by yourself](../installation-config)
* The .nunit file: It references the config file and the NBi framework. You need to [initialize it by yourself](../installation-nunit-project)

In each of these files, you will find the following content:

![repository structure](../../img/docs/installation-test-environment/intro_03.png)

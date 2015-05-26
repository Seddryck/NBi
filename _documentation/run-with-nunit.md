---
layout: documentation
title: Run test-suite with NUnit
prev_section: installation-nunit-project
next_section: run-alterative
permalink: /docs/run-with-nunit/
---
Once you've created a test-suite, you need to execute it. The easiest way to do this is by the means of NUnit GUI.

Start the NUnit GUI and choose the menu option *file > open project* then select your NUnit project file.

This project file will read the config file, redirecting to the test-suite and execute NBi. Then NBi will parse and interpret the test-suite

![NUnit open a project](../../img/docs/run-with-nunit/open-project.png)

The tests are built and displayed in NUnit UI (It can take a few seconds, especially if your test-suite has more than a few thousands tests). Now, you just need to select your tests and execute them with NUnit UI by clicking on the button *Run*. NBi will execute the tests and report failures.

![NUnit select tests and run them](../../img/docs/run-with-nunit/select-and-run.png)

Depending of your configuration of NUnit, the checkboxes could not be visible. In this case go to *View > Tree > Show checkboxes* and check this option.

![NUnit show checkboxes](../../img/docs/run-with-nunit/show-checkboxes.png)

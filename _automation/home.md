---
layout: automation
title: Automating the creation of test-suites
next_section: cases-load
permalink: /automation/home/
---
NBi is a powerful tool to write expressive and efficient tests for your BI solutions. But the creation of such test-suites takes a lot of times and is, sometimes, really repetitive. NBi comes with a solution to automate, as much as possible,  the creation of the test-suites.

The main idea for automating the creation of test-suites is to decompose the writing of tests in a few steps:

* Creating of a set of test-cases
* Writing a template for the test
* Applying the template to each test-case
* Defining the settings of your test-suite
* Saving the test-suite

NBi is provided with a UI, named GenBI. This software lets you perform the steps defined above through an easy to use interface.

But NBi goes further, with the definition of a macro language, named genbiL. With this macro language, you can script the different actions that you would execute in the UI and ask GenBI to execute them for you. This approach is really powerful and lets you create test-suites with thousands of tests in just a few minutes. More, it also ensures a bit of cohesion in your test-suite and facilitate the maintenance process and reusibility.

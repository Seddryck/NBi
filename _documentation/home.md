---
layout: documentation
title: Welcome
next_section: installation-tools
permalink: /docs/home/
---
This site aims to be a comprehensive guide to NBi. We’ll cover topics such as getting your test-suite up and running, automating the creation of your test-suites, executing and configuring for various environments, and give you some advice on participating in the future development of NBi itself.

So what is NBi, exactly?
------------------------
NBi is a simple but powerful framework for testing BI solutions. It's designed for Microsoft Business Intelligence platform but it also (partially) supports other platforms. Its scope includes relational databases, olap/cubes databases, etls and reports.

How does it work?
-----------------
NBi is an add-on for [NUnit](http://nunit.org). The main goal of this framework is to let users create tests with a declarative approach based on an Xml syntax. By the means of NBi, you don't need to develop C# code to specify your tests! Either, you don't need Visual Studio to compile your test suite. Just create an Xml file and let the framework interpret it and play your tests.

---
layout: automation
title: Getting started with genbiL
prev_section: source-test-cases-from-query
next_section: comments
permalink: /automation/genbil/
---
**GenbiL** is a language to automate the generation of test suites through Genbi. With *genbiL*, you can script all the actions that you are manually executing each time you want to generate your test suite. These actions could be "open a CSV file for test cases" or "select the correct template" or even "define the settings" and "generate your test suite".

*GenbiL* has a standard approach for a script language:

* Each line of code, corresponds to an action and must end by a "*;*".
* This macro-language is ignoring cases.
* Textual parameters are written between simple quotes.
* All paths are relative to the executable that will generate the test suite (genbi.exe).

The action must start by the definition of what will be impacted (Test cases, Test template, Setting or Test suite), followed by the action (Load, Save, Generate, ...) and finally parameters (file name, column). The descriptions here under explain each valid actions by subject and the needed parameters.

For the moment it's not possible to record a macro when executing actions by yourself through the GUI (but later it will be). You can create your script manually with any text editor (even notepad is enougth).

## Execution

To execute your test suite you've two options:

* Start normally genbi.exe and click on the menu on "Macro" then "Play ..." and select your genbiL script. A new window will open and displays all the actions executed by the macro.
* Start genbi.exe with a parameter corresponding to the name of your genbiL script. Only the new window for macro will open and displays all the actions executed by the macro.

{% highlight xml %}
genbi.exe myScript.genbil
{% endhighlight %}

If you want to avoid this window, with the list of actions performed, you can specify to genbi the -quiet option (short term is -q). If this option is specified then no UI will be displayed.

{% highlight xml %}
genbi.exe myScript.genbil -q
{% endhighlight %}

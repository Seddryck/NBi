---
layout: automation
title: Load a set of test-cases
prev_section: cases-load
next_section: cases-scope
permalink: /automation/cases-save/
---
! Save a set of test-cases
This action lets you save your test-cases as a CSV file. It's useful during the debugging of a macro or to keep track of your manipulation on a set of test-cases.

Note that this action will save the currently scoped set of test-cases, to change the scope on another set of test-cases see the action [Scope (case)].

The syntax after the initial keyword _case_ are the keywords _save_ and _as_ followed by the name of the csv file between quotes.

Sample:
{code:xml}
case save as 'relative path\myfile.csv'
{code:xml}

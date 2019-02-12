---
layout: documentation
title: Scalar
prev_section: resultset-format
next_section: primitive-sequence
permalink: /docs/primitive-scalar/
---
A scalar is the most atomic object considered by NBi. A scalar is equivalent to an atomic value. A scalar has a type that can be defined as *text*, *numeric*, *dateTime* or *boolean*. The default type of a scalar is *text*.

## Text

This is the most common type for a scalar. If no type is specified then this type is effectively used. The exact content of the cell is used during comparison. It means that values “10.0”, “010” and “10” are considered as different when using the type *text*. This is usually what you’ll use when specifying *Key* columns. Pay attention that the default comparison for this type is case-sensitive.

## Numeric

To avoid comparison of textual content, you can use the *numeric* type. The content of the cell is first converted to a numeric (decimal) value using the international format (a dot to separate the decimal part). It means that values *10.0*, *010* and *10* are considered as equal when using this type. This type is useful when you’ve *Value* columns.

## Date and time

The content of the cell is first converted to a DateTime value using the international format (yyyy-mm-dd hh:mm:ss).

## Boolean

The content of the cell is first converted to a boolean value. NBi understands “0” or “false” and “1” or “true” as boolean values.
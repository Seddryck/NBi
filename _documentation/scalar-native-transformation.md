---
layout: documentation
title: Native transformations
prev_section: scalar-transform
next_section: resultset-alteration
permalink: /docs/scalar-native-transformation/
---
## Functions

The list of native transformations is available here under and is organized by type of input value.

### Special values

* ```null-to-empty```: if the current content of the cell is ```null``` replace the content by ```(empty)```
* ```null-to-value```: if the current content of the cell is ```null``` replace the content by ```(value)```
* ```any-to-any```: replaces the content of each cell by ```(any)```
* ```value-to-value```: if the cell's value is not ```null``` will replace the content by ```(value)```
* ```null-to-zero```: if the cell is ```null``` or ```empty``` or ```blank```, it replaces the content by ```0```.
* ```null-to-date(dateTime)```: returns the original date if the value wasn't null or empty else returns the value specified as a parameter. ```dateTime``` must be expressed as string: ```2018-05-09```

### Text

* ```blank-to-empty```: if the current text is ```blank``` (zero or many spaces) replace the content by ```empty```
* ```blank-to-null```: if the current text is ```blank``` (zero or many spaces) replace the content by ```null```
* ```empty-to-null```: if the current text is ```empty``` (length=0) replace the content by ```null```
* ```text-to-without-diacritics```: if the current text contains any accents or diacritics, they are removed
* ```text-to-without-whitespaces```: removes blanks from anywhere within the text. If the text is ```null```, it returns ```null``` but if ```empty``` or ```blank```, it returns ```empty```.
* ```text-to-remove-chars(char)```: removes the defined char from anywhere within the text. If the text is ```null```, it returns ```null``` and if ```empty```, it returns ```empty```. If the original value is ```blank``` and the character to remove is not a whitespace it returns ```blank``` else ```empty```.
* ```text-to-upper```: returns a copy of this text converted to uppercase
* ```text-to-lower```: returns a copy of this text converted to lowercase
* ```html-to-text```: decodes the html to text
* ```text-to-html```: encodes the text to html
* ```text-to-trim```: removes blanks from the beginning and end of the text.
* ```text-to-length```: returns the length of the *text* value of the text. If the text is ```null``` or ```empty```, it returns 0.
* ```text-to-token-count``` and ```text-to-token-count(char)```: returns the count of tokens in the original text. If no sperator is specified (char), it uses the whitespace tokenizer that is considered as one or more letter or digit or hyphen seperated by one or more whitespace. If a separator is specified, a token is identified as a serie of any character separated by one or more instance of the separator. If the current value is ```null``` or ```empty``` or ```blank```, it returns 0.
* ```text-to-token(index)``` and ```text-to-token(index, char)```: returns the token at the position of the index. The first token as an index of 0. This function uses the same tokenizer than defined in ```text-to-token-count```. If the requested token doesn't exist (index greater or equal to the tokens' count), it returns ```null```. The initial value ```null``` and ```empty``` returns ```null``` for any separator and any index. In general, the value ```blank``` returns ```blank``` for the first token except if the separator is a whitespace.
* ```text-to-prefix(string)```: Append the value of *string* before the current value. If the current value is ```null```, the result will be ```null```.
* ```text-to-suffix(string)```: Append the value of *string* after the current value. If the current value is ```null```, the result will be ```null```.
* ```text-to-first-chars(length)```: if the text is longer than the specified length, take the first characters.
* ```text-to-last-chars(length)```: if the text is longer than the specified length, take the last characters.
* ```text-to-skip-first-chars(length)```: returns the text except the *length* first characters. If the text's length is less than the specified length returns an empty string.
* ```text-to-skip-last-chars(length)```: returns the text except the *length* last characters. If the text's length is less than the specified length returns an empty string.
* ```text-to-pad-left(length, character)```: if the text is shorter than the specified length, add the specified character at the beginning of the text until the length of this text is equal to the expected length.
* ```text-to-pad-right(length, character)```: if the text is shorter than the specified length, add the specified character at the end of the text until the length of this text is equal to the expected length.
* ```text-to-dateTime(format)``` and ```text-to-dateTime(format, culture)``` returns a dateTime from the text value after parsing it with the *format* provided as argument. If the format includes day or month names, it could be useful to specify the *culture*.
* ```text-to-mask(format)``` returns a formated text based on a mask. The mask is a text where the character '*' will be replaced by a glyph from the original text, other characters of the mask are unreplaced. As an example the text `12345678` with a mask `BE-***.***.***` will return the textual value `BE-123.456.78`. If the mask is expecting less charachters than the original value then the remaining characters are dropped. If the mask is expecting more components than the original value the last '*' characters won't be not replaced.
* ```mask-to-text(format)``` returns a unformated text extracted from a text on which a mask was previously applied. The mask is a text where the character '*' has been replaced by a glyph from the original text, other characters of the mask are not substitued. As an example the masked text `BE-123.456.78` with a mask `BE-***.***.***` will return the textual value `12345678`. In case this transformation can't happen because the masked value and the mask don't match, the retruned result is `(null)`.


### Numeric

* ```numeric-to-floor```: returns the largest integral value less than or equal to the specified number. If the cell is ```null``` or ```empty``` or ```blank```, it returns ```null```.
* ```numeric-to-ceiling```: returns the smallest integral value greater than or equal to the specified number. If the cell is ```null``` or ```empty``` or ```blank```, it returns ```null```.
* ```numeric-to-integer```: rounds a value to the nearest integer. If the cell is ```null``` or ```empty``` or ```blank```, it returns ```null```.
* ```numeric-to-round(integer)```: rounds a value to the specified number of fractional digits.
* ```numeric-to-clip(numeric, numeric)```: Clip a value such as if smaller than the first argument then it will return the first argument or if larger than the second argument then will return the second argument. If the original value is between the first and second argument then the original value is returned.
* ```numeric-to-increment```: add 1 to the current value
* ```numeric-to-decrement```: subtract 1 to the current value
* ```numeric-to-add(incr)```: add *incr* to the current value
* ```numeric-to-add(incr, times)```: add *incr* to the current value several *times*. If *times* is zero, return current value. *times* must be an integer value.
* ```numeric-to-subtract(decr)```: subtract *decr* to the current value
* ```numeric-to-subtract(decr, times)```: subtract *decr* to the current value several *times*. If *times* is zero, return current value. *times* must be an integer value.
* ```numeric-to-multiply(factor)```: multiply the current value by *factor*
* ```numeric-to-divide(factor)```: divide the current value by *factor*
* ```numeric-to-invert```: invert the current value (equivalent to *1/current value*)

### DateTime

* ```date-to-age```: returns the age according to the *dateTime* value of the cell at the moment of execution of the test.
* ```dateTime-to-date```: remove information about the time (equivalent to set the dateTime to midnight)
* ```dateTime-to-first-of-month```: returns the first day of the month where the given date lies in.
* ```dateTime-to-first-of-year```: returns the first day of the year where the given date lies in.
* ```dateTime-to-last-of-month```: returns the last day of the month where the given date lies in.
* ```dateTime-to-last-of-year```: returns the last day of the year where the given date lies in.
* ```dateTime-to-next-day```: returns the next day, at the same time.
* ```dateTime-to-previous-day```: returns the previous day, at the same time.
* ```dateTime-to-next-month```: returns a dateTime corresponding to one month after the given date.
* ```dateTime-to-previous-month```: returns a dateTime corresponding to one month before the given date.
* ```dateTime-to-next-year```: returns a dateTime corresponding to one year after the given date.
* ```dateTime-to-previous-year```: returns a dateTime corresponding to one year before the given date.
* ```dateTime-to-floor-hour```: returns a dateTime rounded down to the nearest boundary of an hour.
* ```dateTime-to-ceiling-hour```: returns a dateTime rounded up to the nearest boundary of an hour.
* ```dateTime-to-floor-minute```: returns a dateTime rounded down to the nearest boundary of a minute.
* ```dateTime-to-ceiling-minute```: returns a dateTime rounded up to the nearest boundary of a minute.
* ```dateTime-to-clip(dateTime, dateTime)```: Clip a value such as if smaller than the first argument then it will return the first argument or if larger than the second argument then will return the second argument. If the original value is between the first and second argument then the original value is returned.
* ```dateTime-to-set-time(timeSpan)```: Set the hours, minutes, second of a dateTime to the specified value without changing the date part. The timespan should be defined with the format *hh:mm:ss* such as ```07:00:00```.
* ```dateTime-to-add(ts)```: add *ts* (a timeSpan) to the current value
* ```dateTime-to-add(ts, times)```: add *ts* (a timeSpan) to the current value several times. If times is zero, return current value. *times* must be an integer value.
* ```dateTime-to-subtract(ts)```: subtract *ts* (a timeSpan) to the current value
* ```dateTime-to-subtract(ts, times)```: subtract *ts* (a timeSpan) to the current value several times. If times is zero, return current value. *times* must be an integer value.
* ```utc-to-local(timeZone)```: returns the dateTime converted from UTC to the local time of the specified time zone
* ```local-to-utc(timeZone)```: returns the dateTime converted from the local time of the specified time zone to utc. If the local time was ambiguous (at the moment of the switch between summer and winter the same local time occurs twice) then the first occurance is selected.

### Path and file transformations

The following transformations will consider the location of the test-suite as the base path when facing a relative path.

* ```path-to-filename```:  returns the file name and extension of the specified path string. The characters after the last directory separator character in path. If the last character of path is a directory or volume separator character, this method returns ```empty```.
* ```path-to-filename-without-extension```: Returns the file name of the specified path string without the extension. The text returned by ```path-to-filename```, minus the last period (.) and all characters following it.
* ```path-to-extension```: Returns the extension (including the period ".") of the specified path string. The extension of the specified path (including the period ".") or ```(empty)```.
* ```path-to-root```: Gets the root directory information of the specified path including a directory separator character at the end.
* ```path-to-directory```: returns the directory information for the specified path string. 

For these transformations, the input must corresponds to an existing file. If it's not the case an exception will be generated and the test will fail.

* ```file-to-size```: Gets the size, in bytes, of the file.
* ```file-to-creation-dateTime```: Gets the creation time of the file.
* ```file-to-creation-dateTime-utc```: Gets the creation time, in coordinated universal time (UTC), of the file.
* ```file-to-update-dateTime```: Gets the time that the current file was last written to.
* ```file-to-update-dateTime-utc```: Gets the time, in coordinated universal time (UTC), that the current file was last written to.

## Parameters

Some native transformations except parameters to operate. You must replace the information beween parenthesis with a string matching your expectation.

### Parameter's type

* timeZone: represents a valid TimeZone. User must specify the identification of a time zone (Romance Standard Time ...) or the name of one of the city listed in the display of a TimeZone (Brussels, Paris ...).
* timeSpan represents a duration expressed in days, hours, minutes, seconds. The format is ```d.HH:mm:ss``` where the count of day is facultative (```HH:mm:ss```)
* dateTime represents a date and its time. The format is ```yyyy-MM-dd HH:mm:ss```
* integer: represents a numeric value
* character: represents a unique character

### Parameter's value

The easiest way to specify the parameter's value is to use the literal value and specify it directly such as in ```dateTime-to-subtract(00:15:00)```. Note that this value is not surrounded by quotes or double-quotes.

It's also possible to define the parameteres value with a variable (global or instance). To achieve this, you'll need to use the variable's name prefixed by the ```@``` symbol such as in ```numeric-to-add(@incr)```.

Finally, when the native transformation is applied in a contextual zone (usually in a result-set). You can also define it with a colmn's name or column's ordinal. The value will correspond to the value of this column for the current row evaluated: ```numeric-to-add([incr], #1)```.

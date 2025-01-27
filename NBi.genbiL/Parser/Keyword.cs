using System;
using System.Linq;
using Sprache;

namespace NBi.GenbiL.Parser;

class Keyword
{
    public static readonly Parser<string> Case = Parse.IgnoreCase("Case").Text().Token();
    public static readonly Parser<string> Template = Parse.IgnoreCase("Template").Text().Token();
    public static readonly Parser<string> Setting = Parse.IgnoreCase("Setting").Text().Token();
    public static readonly Parser<string> Suite = Parse.IgnoreCase("Suite").Text().Token();
    public static readonly Parser<string> Consumable = Parse.IgnoreCase("Consumable").Text().Token();
    public static readonly Parser<string> Variable = Parse.IgnoreCase("Variable").Text().Token();
    public static readonly Parser<string> CsvProfile = Parse.IgnoreCase("Csv-Profile").Text().Token();

    public static readonly Parser<string> Load = Parse.IgnoreCase("Load").Text().Token();
    public static readonly Parser<string> Add = Parse.IgnoreCase("Add").Text().Token();
    public static readonly Parser<string> Clear = Parse.IgnoreCase("Clear").Text().Token();
    public static readonly Parser<string> Remove = Parse.IgnoreCase("Remove").Text().Token();
    public static readonly Parser<string> Hold = Parse.IgnoreCase("Hold").Text().Token();
    public static readonly Parser<string> Move = Parse.IgnoreCase("Move").Text().Token();
    public static readonly Parser<string> Rename = Parse.IgnoreCase("Rename").Text().Token();
    public static readonly Parser<string> Generate = Parse.IgnoreCase("Generate").Text().Token();
    public static readonly Parser<string> Save = Parse.IgnoreCase("Save").Text().Token();
    public static readonly Parser<string> Filter = Parse.IgnoreCase("Filter").Text().Token();
    public static readonly Parser<string> Scope = Parse.IgnoreCase("Scope").Text().Token();
    public static readonly Parser<string> Cross = Parse.IgnoreCase("Cross").Text().Token();
    public static readonly Parser<string> Copy = Parse.IgnoreCase("Copy").Text().Token();
    public static readonly Parser<string> Set = Parse.IgnoreCase("Set").Text().Token();
    public static readonly Parser<string> Merge = Parse.IgnoreCase("Merge").Text().Token();
    public static readonly Parser<string> Include = Parse.IgnoreCase("Include").Text().Token();
    public static readonly Parser<string> Replace = Parse.IgnoreCase("Replace").Text().Token();
    public static readonly Parser<string> Concatenate = Parse.IgnoreCase("Concatenate").Text().Token();
    public static readonly Parser<string> Substitute = Parse.IgnoreCase("Substitute").Text().Token();
    public static readonly Parser<string> AddRange = Parse.IgnoreCase("AddRange").Text().Token();
    public static readonly Parser<string> Separate = Parse.IgnoreCase("Separate").Text().Token();
    public static readonly Parser<string> Group = Parse.IgnoreCase("Group").Text().Token();
    public static readonly Parser<string> Reduce = Parse.IgnoreCase("Reduce").Text().Token();
    public static readonly Parser<string> Split = Parse.IgnoreCase("Split").Text().Token();
    public static readonly Parser<string> Duplicate = Parse.IgnoreCase("Duplicate").Text().Token();
    public static readonly Parser<string> Trim = Parse.IgnoreCase("Trim").Text().Token();

    public static readonly Parser<string> Into = Parse.IgnoreCase("Into").Text().Token();
    public static readonly Parser<string> To = Parse.IgnoreCase("To").Text().Token();
    public static readonly Parser<string> On = Parse.IgnoreCase("On").Text().Token();
    public static readonly Parser<string> Distinct = Parse.IgnoreCase("Distinct").Text().Token();
    public static readonly Parser<string> In = Parse.IgnoreCase("In").Text().Token();
    public static readonly Parser<string> Out = Parse.IgnoreCase("Out").Text().Token();
    public static readonly Parser<string> Not = Parse.IgnoreCase("Not").Text().Token();
    public static readonly Parser<string> With = Parse.IgnoreCase("With").Text().Token();
    public static readonly Parser<string> As = Parse.IgnoreCase("As").Text().Token();
    public static readonly Parser<string> Value = Parse.IgnoreCase("Value").Text().Token();
    public static readonly Parser<string> Values = Parse.IgnoreCase("Values").Text().Token();
    public static readonly Parser<string> Vector = Parse.IgnoreCase("Vector").Text().Token();
    public static readonly Parser<string> Column = Parse.IgnoreCase("Column").Text().Token();
    public static readonly Parser<string> Columns = Parse.IgnoreCase("Columns").Text().Token();
    public static readonly Parser<string> File = Parse.IgnoreCase("File").Text().Token();
    public static readonly Parser<string> Predefined = Parse.IgnoreCase("Predefined").Text().Token();
    public static readonly Parser<string> Embedded = Parse.IgnoreCase("Embedded").Text().Token();
    public static readonly Parser<string> Folder = Parse.IgnoreCase("Folder").Text().Token();
    public static readonly Parser<string> When = Parse.IgnoreCase("When").Text().Token();
    public static readonly Parser<string> Left = Parse.IgnoreCase("Left").Text().Token();
    public static readonly Parser<string> Right = Parse.IgnoreCase("Right").Text().Token();
    public static readonly Parser<string> All = Parse.IgnoreCase("All").Text().Token();
    public static readonly Parser<string> Optional = Parse.IgnoreCase("Optional").Text().Token();

    public static readonly Parser<string> FieldSeparator = Parse.IgnoreCase("Field-Separator").Text().Token();
    public static readonly Parser<string> RecordSeparator = Parse.IgnoreCase("Record-Separator").Text().Token();
    public static readonly Parser<string> TextQualifier = Parse.IgnoreCase("Text-Qualifier").Text().Token();
    public static readonly Parser<string> FirstRowHeader = Parse.IgnoreCase("First-Row-Header").Text().Token();
    public static readonly Parser<string> EmptyCell = Parse.IgnoreCase("Empty-Cell").Text().Token();
    public static readonly Parser<string> MissingCell = Parse.IgnoreCase("Missing-Cell").Text().Token();
}

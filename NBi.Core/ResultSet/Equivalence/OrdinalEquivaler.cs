using System;
using System.Collections.Generic;
using System.Data;
using NBi.Core.Scalar.Comparer;
using NBi.Core.ResultSet.Analyzer;
using NBi.Extensibility;

namespace NBi.Core.ResultSet.Equivalence;

public class OrdinalEquivaler : BaseEquivaler
{
    private new SettingsOrdinalResultSet? Settings
        => (SettingsOrdinalResultSet?) base.Settings;

    public OrdinalEquivaler(IEnumerable<IRowsAnalyzer> analyzers, SettingsOrdinalResultSet? settings = null)
        : base(analyzers, settings)
    { }

    protected override void PreliminaryChecks(IResultSet x, IResultSet y)
    {
        var columnsCount = Math.Max(y.ColumnCount, x.ColumnCount);
        if (Settings == null)
            BuildDefaultSettings(columnsCount);
        else
            Settings.ApplyTo(columnsCount);

        WriteSettingsToDataTableProperties(y, Settings ?? throw new InvalidOperationException());
        WriteSettingsToDataTableProperties(x, Settings);

        CheckSettingsAndDataTable(y, Settings);
        CheckSettingsAndDataTable(x, Settings);

        CheckSettingsAndFirstRow(y, Settings);
        CheckSettingsAndFirstRow(x, Settings);
    }

    public override EngineStyle Style
    {
        get => EngineStyle.ByIndex;
    }

    protected override DataRowKeysComparer BuildDataRowsKeyComparer(IResultSet x)
    {
        if (Settings == null)
            BuildDefaultSettings(x.ColumnCount);
        return new DataRowKeysComparerByOrdinal(Settings!, x.ColumnCount);
    }

    protected override bool CanSkipValueComparison()
        => Settings is not null && Settings.KeysDef == SettingsOrdinalResultSet.KeysChoice.All;

    protected override IResultRow? CompareRows(IResultRow rx, IResultRow ry)
    {
        if (Settings == null)
            BuildDefaultSettings(rx.ColumnCount);

        var isRowOnError = false;
        for (int i = 0; i < rx.Parent.ColumnCount; i++)
        {
            if (Settings!.GetColumnRole(i) == ColumnRole.Value)
            {
                var x = rx.IsNull(i) ? DBNull.Value : rx[i];
                var y = ry.IsNull(i) ? DBNull.Value : ry[i];
                var rounding = Settings.IsRounding(i) ? Settings.GetRounding(i) : null;
                var result = CellComparer.Compare(y!, x!, Settings.GetColumnType(i), Settings.GetTolerance(i), rounding);

                if (!result.AreEqual)
                {
                    rx.SetColumnError(i, result.Message);
                    if (!isRowOnError)
                        isRowOnError = true;
                }
            }
        }
        if (isRowOnError)
            return rx;
        else
            return null;
    }

    protected void WriteSettingsToDataTableProperties(IResultSet dt, SettingsOrdinalResultSet settings)
    {
        foreach (var column in dt.Columns)
        {
            column.SetProperties(
                settings.GetColumnRole(column.Ordinal)
                , settings.GetColumnType(column.Ordinal)
                , settings.GetTolerance(column.Ordinal)
                , settings.GetRounding(column.Ordinal)
            );
        }
    }

    protected void CheckSettingsAndDataTable(IResultSet dt, SettingsOrdinalResultSet settings)
    {
        var max = settings.GetMaxColumnOrdinalDefined();
        if (dt.ColumnCount <= max)
        {
            var exception = string.Format("You've defined a column with an index of {0}, meaning that your result set would have at least {1} columns but your result set has only {2} columns."
                , max
                , max + 1
                , dt.ColumnCount);

            if (dt.ColumnCount == max && settings.GetMinColumnOrdinalDefined() == 1)
                exception += " You've no definition for a column with an index of 0. Are you sure you'vent started to index at 1 in place of 0?";

            throw new EquivalerException(exception);
        }
    }

    protected void CheckSettingsAndFirstRow(IResultSet dt, SettingsOrdinalResultSet settings)
    {
        if (dt.RowCount == 0)
            return;

        var dr = dt[0];
        for (int i = 0; i < dt.ColumnCount; i++)
        {
            CheckSettingsFirstRowCell(
                    settings.GetColumnRole(i)
                    , settings.GetColumnType(i)
                    , dt.GetColumn(i) ?? throw new InvalidOperationException()
                    , dr.IsNull(i) ? DBNull.Value : dr[i] ?? throw new InvalidOperationException()
                    ,
                        [
                            "The column with index '{0}' is expecting a numeric value but the first row of your result set contains a value '{1}' not recognized as a valid numeric value or a valid interval."
                            , " Aren't you trying to use a comma (',' ) as a decimal separator? NBi requires that the decimal separator must be a '.'."
                            , "The column with index '{0}' is expecting a 'date & time' value but the first row of your result set contains a value '{1}' not recognized as a valid date & time value."
                        ]
            );
        }
    }

    protected virtual void BuildDefaultSettings(int columnsCount)
    {
        base.Settings = new SettingsOrdinalResultSet(
            columnsCount,
            SettingsOrdinalResultSet.KeysChoice.AllExpectLast,
            SettingsOrdinalResultSet.ValuesChoice.Last);
    }

}

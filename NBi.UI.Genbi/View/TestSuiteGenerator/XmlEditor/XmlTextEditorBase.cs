using System;
using System.Drawing;
using ICSharpCode.TextEditor;
using ICSharpCode.TextEditor.Document;

namespace NBi.UI.Genbi.View.TestSuiteGenerator.XmlEditor
{
    public class XmlTextEditorBase : TextEditorControl
    {
        // Methods
        public XmlTextEditorBase()
        {
            base.Document.HighlightingStrategy = HighlightingStrategyFactory.CreateHighlightingStrategy("XML");
            base.Document.FoldingManager.FoldingStrategy = new XmlFoldingStrategy();
            base.Document.FormattingStrategy = new XmlFormattingStrategy();
            base.TextEditorProperties = InitializeProperties();
            base.Document.FoldingManager.UpdateFoldings(string.Empty, null);
            base.ActiveTextAreaControl.TextArea.Enabled = false;
        }

        private static ITextEditorProperties InitializeProperties()
        {
            var properties = new DefaultTextEditorProperties();
            properties.Font = new Font("Courier new", 9, FontStyle.Regular);
            properties.IndentStyle = IndentStyle.Smart;
            properties.ShowSpaces = false;
            properties.LineTerminator = Environment.NewLine;
            properties.ShowTabs = false;
            properties.ShowInvalidLines = false;
            properties.ShowEOLMarker = false;
            properties.TabIndent = 2;
            properties.CutCopyWholeLine = true;
            properties.LineViewerStyle = LineViewerStyle.None;
            properties.MouseWheelScrollDown = true;
            properties.MouseWheelTextZoom = true;
            properties.AllowCaretBeyondEOL = false;
            properties.AutoInsertCurlyBracket = true;
            properties.BracketMatchingStyle = BracketMatchingStyle.After;
            properties.ConvertTabsToSpaces = false;
            properties.ShowMatchingBracket = true;
            properties.EnableFolding = true;
            properties.ShowVerticalRuler = false;
            properties.IsIconBarVisible = true;
            properties.Encoding = System.Text.Encoding.Unicode;
            return properties;
        }

    }
}

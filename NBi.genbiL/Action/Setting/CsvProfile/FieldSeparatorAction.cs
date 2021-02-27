using System;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Text;
using NBi.GenbiL.Stateful;
using NBi.Xml.Settings;

namespace NBi.GenbiL.Action.Setting.CsvProfile
{
    public class FieldSeparatorAction : ICsvProfileAction
    {
        public char Value { get; set; }

        public FieldSeparatorAction(char value)
            => Value = value;

        public void Execute(GenerationState state)
        {
            state.Settings.CsvProfile = (state.Settings.CsvProfile ?? new CsvProfileXml());
            state.Settings.CsvProfile.FieldSeparator = Value;
        }

        public string Display => $"Create CSV profile setting named 'FieldSeparator' and defining it to '{Value}'";
    }
}

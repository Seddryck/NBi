using System;
using System.Linq;
using NBi.UI.Genbi.Command;
using NBi.UI.Genbi.Command.Macro;
using NBi.UI.Genbi.View.TestSuiteGenerator;

namespace NBi.UI.Genbi.Presenter
{
    class MacroPresenter : PresenterBase
    {
        public MacroPresenter()
            : base()
        {
            this.PlayMacroCommand = new PlayMacroCommand(new MacroWindow());
        }

        public ICommand PlayMacroCommand { get; private set; }

        #region Bindable properties

        #endregion
    }
}

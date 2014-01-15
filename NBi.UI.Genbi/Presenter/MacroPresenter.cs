using System;
using System.Linq;
using NBi.UI.Genbi.Command;
using NBi.UI.Genbi.Command.Macro;
using NBi.UI.Genbi.Interface;

namespace NBi.UI.Genbi.Presenter
{
    class MacroPresenter : BasePresenter<IMacroView>
    {
        public MacroPresenter(IMacroView macroView)
            : base(macroView)
        {
            this.PlayMacroCommand = new PlayMacroCommand();
        }

        public ICommand PlayMacroCommand { get; private set; }

        #region Bindable properties

        #endregion
    }
}

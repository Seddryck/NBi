using System;
using System.Linq;
using NBi.UI.Genbi.Interface;

namespace NBi.UI.Genbi.Presenter
{
    public class BasePresenter<TView> where TView : class, IView
    {
        public TView View { get; private set; }

        public BasePresenter(TView view)
        {
            if (view == null)
                throw new ArgumentNullException("view");

            View = view;
            View.Initialize += OnViewInitialize;
            //View.Load += OnViewLoad;
        }

        protected virtual void OnViewInitialize(object sender, EventArgs e) { }

        protected virtual void OnViewLoad(object sender, EventArgs e) { }
    }
}

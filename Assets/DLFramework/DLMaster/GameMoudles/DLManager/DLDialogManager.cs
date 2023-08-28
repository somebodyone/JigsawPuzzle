
using System.Collections.Generic;

namespace DLBASE
{
    public class DLDialogManager :DLSingleton<DLDialogManager>
    {
        private List<DLDialog> _views = new List<DLDialog>();

        public T OpenDialog<T>(params object[] args) where T : new()
        {
            T t = new T();
            DLDialog dialog = t as DLDialog;
            dialog.OnInit();
            dialog.InitData(args);
            dialog.Init();
            _views.Add(dialog);
            return t;
        }

        public void CloseDialog<T>() where T : DLDialog
        {
            for (int i = 0; i < _views.Count; i++)
            {
                if (typeof(T)== _views[i].GetType())
                {
                    _views[i].Close();
                    _views.Remove(_views[i]);
                    return;
                }
            }
        }
    }
}

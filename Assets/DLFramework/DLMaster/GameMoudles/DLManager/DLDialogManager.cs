
using System.Collections.Generic;

namespace DLBASE
{
    public class DLDialogManager :DLSingleton<DLDialogManager>
    {
        private List<DLDialog> _views = new List<DLDialog>();

        public T OpenView<T>() where T : new()
        {
            T t = new T();
            DLDialog dialog = t as DLDialog;
            dialog.OnInit();
            dialog.Init();
            _views.Add(dialog);
            return t;
        }

        public void Close<T>() where T : IFView
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

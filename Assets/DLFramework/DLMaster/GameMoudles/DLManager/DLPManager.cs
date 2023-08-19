using System;
using DLAM;

namespace DLBASE
{
    public class DLPManager:DLSingleton<DLPManager>
    {
        public T GetPresenter<T>() where T:DLPresenter
        {
            var t = typeof(T);
            DLPresenter obj =(DLPresenter) Activator.CreateInstance(t);
            obj.OnInit();
            return (T)obj;
        }
    }
}
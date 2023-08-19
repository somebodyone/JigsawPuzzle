namespace DLBASE
{
    public abstract class IPresenter<T>: DLPresenter where T:DLPresenter
    {
        private static T _instance;
        public static T Instance => _instance ??= DLPManager.Instance.GetPresenter<T>();
        public abstract override void OnInit();
    }
}
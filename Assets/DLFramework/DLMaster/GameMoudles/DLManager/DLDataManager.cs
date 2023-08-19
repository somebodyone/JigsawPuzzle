namespace DLBASE
{
    public class DLDataManager:DLSingleton<DLDataManager>
    {
        public static DLOpition<T> GetOpition<T>()where T:new ()
        {
            DLOpition<T> opition = new DLOpition<T>();
            opition.data = new T();
            return opition;
        }
    }
}
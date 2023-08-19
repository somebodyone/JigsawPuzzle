namespace DLBASE
{
    public class DLSingleton<T>where T:new ()
    {
        private static T Ins;

        public static T Instance
        {
            get
            {
                if (Ins == null)
                {
                    Ins = new T();
                }

                return Ins;
            }
        }
    }
}
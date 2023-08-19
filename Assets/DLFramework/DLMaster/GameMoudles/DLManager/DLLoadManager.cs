using FairyGUI;

namespace DLBASE
{
    public class DLLoadManager:DLSingleton<DLLoadManager>
    {
        public static void LoadPakege(string url)
        {
            UIPackage.AddPackage("fairygui/"+url);
        }
    }
}
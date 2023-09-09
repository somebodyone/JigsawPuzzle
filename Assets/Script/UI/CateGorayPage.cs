using DLBASE;
using FairyGUI;

namespace DLAM
{
    public class CateGorayPage:GComponent
    {
        public static string URL = "ui://Main/图片筛选";
        private GList _list;
        private CateGorayData _data => CateGorayPresenter.Instance.GetData();

        public void OnInit()
        {
            _list = GetChild("list").asList;
            for (int i = 0; i < _data.datas.Count; i++)
            {
                CateGorayItemData data = _data.datas[i];
                GButton label = UIPackage.CreateObjectFromURL("ui://Main/子项").asButton;
                label.icon = "ui://Main/item" + data.type;
                label.title = data.namekey;
                _list.AddChild(label);
                label.onClick.Add(() => { DLDialogManager.Instance.OpenDialog<SelectPicDialog>(data);});
            }
        }
    }
}
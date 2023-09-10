using System.Collections.Generic;
using DLBASE;
using FairyGUI;

namespace DLAM
{
    public class SelectDifficultyDialog : DLDialogX<SelectDifficultyDialog>
    {
        private GButton _close;
        private GButton _enter;
        private PhotoData _data;
        private GLabel _icon;
        private GList _list;
        private List<DiffctyCom> _listitems = new List<DiffctyCom>();
        private int _selectid = 0;

        public override void OnInit()
        {
            SetContentWith("Dialog", "游戏进入选择界面");
        }

        public override void InitData(params object[] args)
        {
            _data = (PhotoData)args[0];
        }

        protected override void InitCompent()
        {
            _enter = contentPlane.GetChild("enter").asButton;
            _list = contentPlane.GetChild("list").asList;
            _icon = contentPlane.GetChild("title").asLabel;
            _close = contentPlane.GetChild("back").asButton;
            _icon.icon = "ui://Main/" + _data.photoname + _data.id;
            for (int i = 0; i < 3; i++)
            {
                DiffctyCom item = UIPackage.CreateObjectFromURL(DiffctyCom.URL) as DiffctyCom;
                item.OnInit(i + 1,_data.reward[i]);
                item.AddClick((compent) =>
                {
                    compent.Select(true);
                    _listitems[_selectid].Select(false);
                    _selectid = compent.ID - 1;
                    _data.selecetId = _selectid;
                });
                if (i == 0)
                {
                    item.Select(true);
                }
                _listitems.Add(item);
                _list.AddChild(item);
            }
            _close.onClick.Add(() =>
            {
                contentPlane.TweenMoveX(GRoot.inst.width, 0.3f).OnComplete(() =>
                {
                    DLDialogManager.Instance.CloseDialog<SelectDifficultyDialog>();
                });
            });
            _enter.onClick.Add(() =>
            {
                DLDialogManager.Instance.OpenDialog<GameDialog>(_data);
                DLDialogManager.Instance.CloseDialog<SelectDifficultyDialog>();
            });
        }
    }
}
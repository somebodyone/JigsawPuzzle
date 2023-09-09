using System.Collections.Generic;
using DLBASE;
using FairyGUI;

namespace DLAM
{
    public class SelectPicDialog : DLDialog
    {
        private GButton _back;
        private GList _list;
        private CateGorayItemData _itemdata;
        private List<PhotoData> _datas;
        private GTextField _title;


        public override void OnInit()
        {
            SetContentWith("Dialog", "选图界面");
        }

        public override void InitData(params object[] args)
        {
            _itemdata = (CateGorayItemData)args[0];
            _datas = GamePresenter.Instance.GetData(_itemdata.type);
        }

        protected override void InitCompent()
        {
            _title = contentPlane.GetChild("title").asTextField;
            _back = contentPlane.GetChild("back").asButton;
            _list = contentPlane.GetChild("list").asList;
            _title.text = _itemdata.namekey;
            _back.onClick.Add(() =>
            {
                contentPlane.TweenMoveY(GRoot.inst.height, 0.3f).OnComplete(() =>
                {
                    DLDialogManager.Instance.CloseDialog<SelectPicDialog>();
                });
            });
            for (int i = 0; i < _datas.Count; i++)
            {
                Card card = UIPackage.CreateObjectFromURL(Card.URL) as Card;
                card.Init(_datas[i]);
                _list.AddChild(card);
            }
        }
    }
}
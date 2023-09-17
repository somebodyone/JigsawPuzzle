using FairyGUI;

namespace DLAM
{
    public class MyPuzzleCard:GComponent
    {
        public static string URL = "ui://Main/我的拼图子项";
        private PhotoData _data;
        private GProgressBar _diffcutily_1;
        private GProgressBar _diffcutily_2;
        private GProgressBar _diffcutily_3;
        private GTextField _field_1;
        private GTextField _field_2;
        private GTextField _field_3;
        private GButton _cardbtn;

        public void OnInit(PhotoData data)
        {
            _data = data;
            _cardbtn = GetChild("card").asButton;
            _diffcutily_1 = GetChild("diffculity_1").asProgress;
            _diffcutily_2 = GetChild("diffculity_2").asProgress;
            _diffcutily_3 = GetChild("diffculity_3").asProgress;
            
            _field_1 = GetChild("diffculitytitle_1").asTextField;
            _field_2 = GetChild("diffculitytitle_2").asTextField;
            _field_3 = GetChild("diffculitytitle_3").asTextField;
            _field_1.SetVar("time",(data.times[0]==-1?"Not Started":data.times[0].ToString())+"s").FlushVars();
            _field_2.SetVar("time",(data.times[1]==-1?"Not Started":data.times[1].ToString())+"s").FlushVars();
            _field_3.SetVar("time",(data.times[2]==-1?"Not Started":data.times[2].ToString())+"s").FlushVars();
            _diffcutily_1.value = data.times[0] == -1 ? 0 : 100;
            _diffcutily_2.value = data.times[1] == -1 ? 0 : 100;
            _diffcutily_3.value = data.times[2] == -1 ? 0 : 100;
            _cardbtn.icon = "ui://Main/"+data.photoname + data.id;
        }
        
    }
}
using DLBASE;
using DTT.MiniGame.Jigsaw;
using UnityEngine;

namespace DLAM
{
    public static class DataMgr
    {
        private static JigsawConfig CurLevelData;
        
        public static DLOpition<Data> GetGameDate()
        {
            if (DLDataManager.GetOpition<Data>() != null)
            {
                return DLDataManager.GetOpition<Data>();
            }

            return new DLOpition<Data>();
        }


        public static JigsawConfig GetCurLevelData()
        {
            if (CurLevelData == null)
            {
                CurLevelData =  ScriptableObject.CreateInstance<JigsawConfig>();
                CurLevelData.Size = new Vector2Int(2, 3);
                CurLevelData.Image = Resources.Load<Sprite>("test");

            }

            return CurLevelData;
        }

        public static void SetCurLevelData(Vector2Int size,string imgName)
        {
            CurLevelData.Size = size;

            CurLevelData.Image = Resources.Load<Sprite>(imgName);
        }
    }
}
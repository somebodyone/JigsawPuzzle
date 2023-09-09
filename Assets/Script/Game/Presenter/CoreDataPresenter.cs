using System;
using System.Collections.Generic;
using System.IO;
using DLBASE;
using UnityEngine;

namespace DLAM
{
    
    [Serializable]
    public class GameCoreConfig
    {
        public int version;
        public int subVersion;
        public int minSubVersion;
        public List<string> CateGorayConfig;
    }
    
    public class CoreDataPresenter : IPresenter<CoreDataPresenter>
    {
        public GameCoreConfig GameCoreConfig;
        
        public override void OnInit()
        {
            GameCoreConfig = ReadData<GameCoreConfig>();
        }

        /// <summary>
        /// IO读取
        /// </summary>
        /// <returns></returns>
        private T ReadData<T>()
        {
            TextAsset jsonFile = Resources.Load<TextAsset>("Config/jsonmeta/game_core_config"); // 不需要.json 后缀
            string jsonString = jsonFile.text;
            return JsonUtility.FromJson<T>(jsonString);
        }
    }
}
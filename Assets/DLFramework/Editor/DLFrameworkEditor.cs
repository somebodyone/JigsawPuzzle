using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;


namespace DLBASE
{
    /// <summary>
    /// 框架Editor基类
    /// </summary>
    [CanEditMultipleObjects]
    public class DLFrameworkEditor : Editor
    {
        [MenuItem("DLFramework/清除本地数据")]
        private static void InitDLFramework()
        {
            PlayerPrefs.DeleteAll();
        }
    }
}

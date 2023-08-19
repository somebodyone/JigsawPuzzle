
using System;
using System.Collections;
using System.Collections.Generic;
using DLAM;
using FairyGUI;
using UnityEngine;
using UnityEngine.EventSystems;
using Random = System.Random;

namespace DLBASE
{

    public class GameUtlis : MonoBehaviour
    {
        public enum WaitType
        {
            Once,
            Reseapt
        }

        public static void RandomList(List<int> list, int count,int target)
        {
            for (int i = 0; i < count; i++)
            {
                int random = UnityEngine.Random.Range(0, list.Count);
                list.Insert(random,target);
            }
        }

        public static void RandomArray(List<int> array)
        {
            if (array.Count <= 1) return;
            for (int i = 0; i < 5; i++)
            {
                int id1 = UnityEngine.Random.Range(0, array.Count);
                int id2 = UnityEngine.Random.Range(0, array.Count);
                int temp = array[id1];
                array[id1] = array[id2];
                array[id2] = temp;
            }
        }
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="atomValue"></param>
        /// <param name="keepPointCount"></param> 保留小数点后几位
        /// <returns></returns>

        public static int[] StringToArray(string str)
        {
            str = str.Replace('，', ',');
            string[] strList = str.Split(',');
            int num = 0,len = strList.Length;
            int[] numList = new int[len];
            for (int i = 0,j = 0; i < len; i++)
            {
                if (int.TryParse(strList[i], out num))
                {
                    numList[j++] = num;
                }
                else
                {
                    Debug.Log(str+"===========");
                    Debug.LogError("转换失败"+strList[i]);
                }
            }
            return numList;
        }

        public static Color ChangeColor(string color)
        {
            Color colors;
            ColorUtility.TryParseHtmlString(color,out colors);
            return colors;
        }

        public static Vector2 WorldToUI(Vector3 worldPos)
        {
            Vector3 screenPos = Camera.main.WorldToScreenPoint(worldPos);
            //原点位置转换
            screenPos.y = Screen.height - screenPos.y; 
            Vector2 pt = GRoot.inst.GlobalToLocal(screenPos);
            return pt;
        }

        public static string ChangeColor(Color color)
        {
            string colors = ColorUtility.ToHtmlStringRGBA(color);
            return colors;
        }

        public static Vector3 GetRealPos(Vector3 pos)
        {
            var result = (pos) * new Vector2(Stage.inst.scaleX, Stage.inst.scaleY);
            result.y *= -1;
            return result;
        }

        public static int RandomAmount(int endAmount)
        {
            int next = UnityEngine.Random.Range(0, endAmount);
            return next;
        }
        public static int RandomAmount(int start,int endAmount)
        {
            int next = UnityEngine.Random.Range(start, endAmount);
            return next;
        }
        
        /// <summary>
        /// 射线检测
        /// </summary>
        /// <param name="start"></param>
        /// <returns></returns>
        public static Vector3 RayCastTargetLaser(Vector3 start,Vector3 dir)
        {
            int layerMask =(1 << 9) | (1 << 8);
            RaycastHit2D info = Physics2D.Raycast( start, dir);
            if(info.collider!=null)
            {
                return info.point;
            }

            return Vector3.zero;
        }
        
        
        /// <summary>
        /// 射线检测
        /// </summary>
        public static bool IsRaycast(Vector3 start,Vector3 end,float distance)
        {
            Vector3 dir = Vector3.Normalize(end - start);
            int layerMask =(1 << 9) | (1 << 8);
            RaycastHit2D info = Physics2D.Raycast( start, dir, distance,layerMask);
            if(info.collider!=null){
                Debug.DrawLine(start, end, Color.green);
                if(info.transform.gameObject.CompareTag("Wall")||info.transform.gameObject.CompareTag("Robot")){
                    return true;
                }else{
                    return false;
                }
            }else{
                return false;
            }
        }
        
        /// <summary>
        /// 射线检测
        /// </summary>
        /// <param name="start"></param>
        /// <returns></returns>
        public static bool RayCastTarget(Vector3 start,Vector3 dir)
        {
            int layerMask =(1 << 9) | (1 << 8);
            RaycastHit2D info = Physics2D.Raycast( start, dir, 0.5f,layerMask);
            Debug.DrawLine(start,dir*10);
            if(info.collider!=null){
                if(info.transform.gameObject.CompareTag("Wall")||info.transform.gameObject.CompareTag("Box")||info.transform.gameObject.CompareTag("Robot")){
                    return true;
                }else{
                    return false;
                }
            }else{
                return false;
            }
        }

        public static int[] GetRarray(int[] arr,int id)
        {
            List<int> array = new List<int>();
            for (int i = 0; i < arr.Length; i++)
            {
                if (arr[i] != id)
                {
                    array.Add(arr[i]);
                }
            }
            return array.ToArray();
        }

        public static float RandomAmount(float endAmount)
        {
            float next = UnityEngine.Random.Range(0, endAmount);
            return next;
        }

        private static IEnumerator Wait(float time, Action callback)
        {
            yield return new WaitForSeconds(time);
            callback?.Invoke();
        }

        public static IEnumerator Wait(float time,Vector3 vector3, Vector2Int vector2, Action<Vector3, Vector2Int> callback)
        {
            yield return new WaitForSeconds(time);
            callback?.Invoke(vector3, vector2);
        }

        public static string[] DicToKeyArray(Dictionary<string, string> dic)
        {
            List<string> list = new List<string>();
            foreach (var item in dic.Keys)
            {
                list.Add(item);
            }
            return list.ToArray();
        }

        public static string[] DicToValueArray(Dictionary<string, string> dic)
        {
            List<string> list = new List<string>();
            foreach (var item in dic.Values)
            {
                list.Add(item);
            }
            return list.ToArray();
        }
        public static bool GetArrayIsHaveItem(List<long> list, long target)
        {
            for (int i = 0; i < list.Count; i++)
            {
                if (list[i] == target)
                {
                    return true;
                }
            }
            return false;
        }

        public static string ArrayToString(int[] array)
        {
            string result = string.Empty;
            for (int i = 0; i < array.Length; i++)
            {
                result += array[i];
                if (i < array.Length - 1)
                    result += ",";
            }
            return result;
        }

        public static string ArrayToString(string[] array)
        {
            string result = string.Empty;
            for (int i = 0; i < array.Length; i++)
            {
                result += array[i];
                if (i < array.Length - 1)
                    result += ",";
            }
            return result;
        }

        public static string ArrayToString(long[] array)
        {
            string result = string.Empty;
            for (int i = 0; i < array.Length; i++)
            {
                result += array[i];
                if (i < array.Length - 1)
                    result += ",";
            }
            return result;
        }

        public static string ArrayToString(double[] array)
        {
            string result = string.Empty;
            for (int i = 0; i < array.Length; i++)
            {
                result += array[i];
                if (i < array.Length - 1)
                    result += ",";
            }
            return result;
        }

        /// <summary>
        /// 适配
        /// </summary>
        /// <returns></returns>
        public static Vector2 Adapter()
        {
            float curent = GRoot.inst.width / GRoot.inst.height;
            float target = 1080.0f / 1920.0f;
            float weidth = 0;
            float height = 0;
            if (curent > target)
            {
                weidth = (1920.0f / GRoot.inst.height) * GRoot.inst.width;
                height = 1920;
            }
            else
            {
                weidth = 1080.0f;
                height = (1080.0f / GRoot.inst.width) * GRoot.inst.height;
            }
            return new Vector2(weidth, height);
        }

        public static float RangeLimit(float current,float max,float min)
        {
            if (current > max)
            {
                current = max;
            }
            if (current < min)
            {
                current = min;
            }
            return current;
        }

        public static bool ClickUI()
        {
            if (Application.platform == RuntimePlatform.Android ||
                        Application.platform == RuntimePlatform.IPhonePlayer)
            {
                int fingerId = Input.GetTouch(0).fingerId;
                if (EventSystem.current.IsPointerOverGameObject(fingerId))
                {
                    return true;
                }
            }
            //其它平台
            else
            {
                if (EventSystem.current.IsPointerOverGameObject())
                {
                    return true;
                }
            }
            return false;
        }

        public static bool SearchFormElements<T>(Dictionary<int,T> dics,int key)
        {
            foreach(int keys in dics.Keys)
            {
                if (key == keys)
                {
                    return true;
                }
            }
            return false;
        }

        public static string GetGUID()
        {
            Guid guid = new Guid();
            guid = Guid.NewGuid();
            string str = guid.ToString();
            return str;
        }

        public static Vector3 UIToWorld(Camera uicamera,Vector3 pos)
        {
           Vector3 uiPostion = uicamera.WorldToScreenPoint(pos);
            uiPostion.z = 1f;
            uiPostion = Camera.main.ScreenToWorldPoint(uiPostion);
            uiPostion.z = 0;
            return uiPostion;
        }

        public static Vector3 WorldToUI(Camera uicamera, Vector3 pos)
        {
            Vector3 uiPostion = uicamera.WorldToScreenPoint(pos);
            uiPostion.z = 1f;
            uiPostion = Camera.main.ScreenToWorldPoint(uiPostion);
            uiPostion.z = 0;
            return uiPostion;
        }

        public static int GetID(int id)
        {
            if (id <= 10)
            {
                return id;
            }
            id -= 11;
            int index = id / 10;
            id -= index * 10;
            return id+1;
        }

        public static Vector3 GetRayPos()
        {
            Vector3 pos = Vector3.zero;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, Mathf.Infinity))
            {
                // 如果射线与平面碰撞，打印碰撞物体信息  
                Debug.Log("碰撞对象: " + hit.collider.name);
                Debug.Log(hit.point);
                pos = hit.point;
                // 在场景视图中绘制射线  
                Debug.DrawLine(ray.origin, hit.point, Color.red);
            }
            return pos;
        }

        public static Transform GetRayTarget()
        {
            Transform transform = null;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, Mathf.Infinity))
            {
                transform = hit.collider.transform;
            }
            return transform;
        }

        public static long ExpToLog(long num)
        {
            long target = (long)Mathf.Log(num,2);
            return target;
        }

        public static string GetSuffix(long target)
        {
            string name = "";
            if (target > 31)
            {
                target -= 31;
                name = "M";
            }
            else if (target > 21)
            {
                target -= 21;
                name = "B";
            }
            else if (target > 11)
            {
                target -= 11;
                name = "K";
            }
            return name;
        }
        public static long GetArrayId(long target)
        {
            if (target > 31)
            {
                target -= 31;
                target += 1;
            }
            else if (target > 21)
            {
                target -= 21;
                target += 1;
            }
            else if (target > 11)
            {
                target -= 11;
                target += 1;
            }
            return target;
        }

        public static long GetListMaxItem(List<long> list)
        {
            long id = 0;
            for (int i = 0; i < list.Count; i++)
            {
                if (list[i] > id)
                {
                    id = list[i];
                }
            }
            return id;
        }

        public static float Angle(Vector3 start,Vector3 end)
        {
            Vector3 target = start - end;
            float angle = Vector3.Angle(target, Vector3.right);
            Vector3 cross = Vector3.Cross(start, end);
            if (cross.z > 0)
            {
                angle *= -1;
            }
            angle -= 90;
            return angle;
        }
        public static float SignedAngleBetween(Vector3 a, Vector3 b)
        {
            float angle = Vector3.Angle(a, b);
            if (a.y < 0)
            {
                angle = 360 - angle;
            }
            return angle;
        }
        
        public static string GetTime(int time)
        {
            int m = time/60;
            int s = time%60;
            string min = m >= 10 ? m.ToString() : "0"+m;
            string sec = s >= 10 ? s.ToString() : "0"+s;
            return  min + ": " + sec + "";
        }
        

        public static Vector3[] Path(Vector3 startTrans, Vector3 endTrans, Vector3 center, int segmentNum)
        {
            //segmentNum为int类型，路径点数量，值越大，路径点越多，曲线越平滑
            Vector3[] path = new Vector3[segmentNum];
            for (int i = 0; i < segmentNum; i++)
            {
                var time = (i + 1) / (float)segmentNum;//归化到0~1范围
                path[i] = BezierCurve( startTrans, endTrans, center,time);//使用贝塞尔曲线的公式取得t时的路径点
            }
            return path;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="P0"></param>
        /// <param name="P1"></param>
        /// <param name="P2"></param>
        /// <param name="t">0.0 >= t <= 1.0 </param>
        /// <returns></returns>
        public static Vector3 BezierCurve(Vector3 P0, Vector3 P1, Vector3 P2, float t)
        {
            Vector3 B = Vector3.zero;
            float t1 = (1 - t)*(1 - t);
            float t2 = t*(1 - t);
            float t3 = t*t;
            B = P0*t1 + 2*t2*P1 + t3*P2;
            //B.y = P0.y*t1 + 2*t2*P1.y + t3*P2.y;
            //B.z = P0.z*t1 + 2*t2*P1.z + t3*P2.z;
            return B;
        }
    }

}
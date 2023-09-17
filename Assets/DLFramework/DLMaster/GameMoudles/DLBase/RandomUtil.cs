using System.Collections.Generic;
using UnityEngine;

namespace Master
{
    public static class RandomUtil
    {
        /// <summary>
        /// 是否中奖, value=[0,1]
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static bool IsInChance(double value)
        {
            if (value <= 0)
            {
                return false;
            }

            if (value >= 1)
            {
                return true;
            }

            return (int) (value * 1000000) > Random.Range(0, 1000000);
        }

        /// <summary>
        ///   <para>Return a random integer number between min [inclusive] and max [exclusive] (Read Only).</para>
        /// </summary>
        /// <param name="min"></param>
        /// <param name="max"></param>
        public static int Range(int min, int max)
        {
            return Random.Range(min, max);
        }

        /// <summary>
        ///   <para>Return a random integer number between min [inclusive] and max [inclusive] (Read Only).</para>
        /// </summary>
        /// <param name="min"></param>
        /// <param name="max"></param>
        public static int FromTo(int min, int max)
        {
            return Random.Range(min, max + 1);
        }

        /// <summary>
        ///   <para>Return a random integer number between min [inclusive] and max [inclusive] (Read Only).</para>
        /// </summary>
        /// <param name="min"></param>
        /// <param name="max"></param>
        public static float FromTo(float min, float max)
        {
            return Random.Range(min, max);
        }

        /// <summary>
        /// 随机一个+1或者-1
        /// </summary>
        /// <returns></returns>
        public static int sign => Random.Range(0, 2) == 0 ? -1 : 1;

        /// <summary>
        /// 根据权重列表随机一个序号
        /// </summary>
        /// <param name="array">权重列表</param>
        /// <param name="name">Log的标志</param>
        /// <returns></returns>
        public static int RandomWeightIndex(int[] array, string name)
        {
            var totalKey = 0;
            foreach (var t in array)
            {
                totalKey += t;
            }

            if (totalKey <= 0)
            {
                return 0;
            }

            var randomKey = Random.Range(1, totalKey + 1);
            var resultIndex = 0;
            totalKey = 0;
            for (var i = 0; i < array.Length; i++)
            {
                totalKey += array[i];
                if (randomKey <= totalKey)
                {
                    resultIndex = i;
                    break;
                }
            }
            
            return resultIndex;
        }

        /// <summary>
        /// 根据权重列表随机返回值
        /// </summary>
        /// <param name="array">权重列表</param>
        /// <param name="name">Log的标志</param>
        /// <returns></returns>
        public static int RandomWeightValue(int[] array, string name)
        {
            var index = RandomWeightIndex(array, name);

            return array[index];
        }


        public static float value => Random.value;

        /// <summary>
        /// 50%概率随机A或者B
        /// </summary>
        /// <returns></returns>
        public static T RandomAorB<T>(T a, T b)
        {
            return sign == 1 ? a : b;
        }

        public static T ArrayRandom<T>(T[] a)
        {
            return a[Random.Range(0, a.Length)];
        }

        public static T ArrayRandom<T>(List<T> a)
        {
            return a[Random.Range(0, a.Count)];
        }

        public delegate int Weight<in T>(T x);
        public delegate double WeightD<in T>(T x);
        public delegate int[] WeightArray<in T>(T x);
        
        /// <summary>
        ///根据添加参数获取适当对象
        /// </summary>
        /// <param name="array">数值对象列表</param>
        /// <param name="conditionFun">数值对象的参数变量</param>
        /// <param name="condition">条件参数</param>
        /// <param name="name">log名字</param>
        /// <typeparam name="T">自定义数值对象</typeparam>
        /// <returns>适配的数值对象</returns>
        public static T GetSuitData<T>(T[] array, Weight<T> conditionFun, int condition, string name)
        {
            var result = array[0];
            for (var i = array.Length - 1; i >= 0; i--)
            {
                var item = array[i];
                if (condition >= conditionFun(item))
                {
                    result = item;
                    break; 
                }
            }
            return result;
        }
        
        /// <summary>
        ///根据添加参数获取适当对象
        /// </summary>
        /// <param name="list">数值对象列表</param>
        /// <param name="conditionFun">数值对象的参数变量</param>
        /// <param name="condition">条件参数</param>
        /// <param name="name">log名字</param>
        /// <typeparam name="T">自定义数值对象</typeparam>
        /// <returns>适配的数值对象</returns>
        public static T GetSuitData<T>(List<T> list, Weight<T> conditionFun, int condition, string name)
        {
            var result = list[0];
            for (var i = list.Count - 1; i >= 0; i--)
            {
                var item = list[i];
                if (condition >= conditionFun(item))
                {
                    result = item;
                    break; 
                }
            }
            return result;
        }
        
        /// <summary>
        ///根据添加参数获取适当对象
        /// </summary>
        /// <param name="array">数值对象列表</param>
        /// <param name="conditionFun">数值对象的参数变量</param>
        /// <param name="condition">条件参数</param>
        /// <param name="name">log名字</param>
        /// <typeparam name="T">自定义数值对象</typeparam>
        /// <returns>适配的数值对象</returns>
        public static T GetSuitData<T>(T[] array, WeightD<T> conditionFun, double condition, string name)
        {
            var result = array[0];
            for (var i = array.Length - 1; i >= 0; i--)
            {
                var item = array[i];
                if (condition >= conditionFun(item))
                {
                    result = item;
                    break; 
                }
            }
            return result;
        }
        
        /// <summary>
        ///根据添加参数获取适当对象
        /// </summary>
        /// <param name="list">数值对象列表</param>
        /// <param name="conditionFun">数值对象的参数变量</param>
        /// <param name="condition">条件参数</param>
        /// <param name="name">log名字</param>
        /// <typeparam name="T">自定义数值对象</typeparam>
        /// <returns>适配的数值对象</returns>
        public static T GetSuitData<T>(List<T> list, WeightD<T> conditionFun, double condition, string name)
        {
            var result = list[0];
            for (var i = list.Count - 1; i >= 0; i--)
            {
                var item = list[i];
                if (condition >= conditionFun(item))
                {
                    result = item;
                    break; 
                }
            }
            return result;
        }
        
        /// <summary>
        /// 根据条件值获取对应档位对象，获取权重ID对应的含义数值
        /// </summary>
        /// <param name="array">数值对象列表</param>
        /// <param name="conditionFun">数值对象的参数变量</param>
        /// <param name="conditionArrayFun">数值对象的权重列表</param>
        /// <param name="ratioArray">权重含义列表</param>
        /// <param name="condition">参数条件</param>
        /// <param name="name">log名字</param>
        /// <typeparam name="T">自定义数值对象</typeparam>
        /// <returns>合适档位对应权重</returns>
        public static int GetRandomByWeight<T>(T[] array, Weight<T> conditionFun, WeightArray<T> conditionArrayFun, int[] ratioArray,int condition, string name)
        {
            var bean = GetSuitData(array, conditionFun, condition, name);
            return GetRandomByWeight(conditionArrayFun(bean), ratioArray, name);
        }
        
        /// <summary>
        /// 根据条件值获取对应档位对象，获取权重ID对应的含义数值
        /// </summary>
        /// <param name="array">数值对象列表</param>
        /// <param name="conditionFun">数值对象的参数变量</param>
        /// <param name="conditionArrayFun">数值对象的权重列表</param>
        /// <param name="ratioArray">权重含义列表</param>
        /// <param name="condition">参数条件</param>
        /// <param name="name">log名字</param>
        /// <typeparam name="T">自定义数值对象</typeparam>
        /// <returns>合适档位对应权重</returns>
        public static double GetRandomByWeight<T>(T[] array, Weight<T> conditionFun, WeightArray<T> conditionArrayFun, double[] ratioArray,int condition, string name)
        {
            var bean = GetSuitData(array, conditionFun, condition, name);
            return GetRandomByWeight(conditionArrayFun(bean), ratioArray, name);
        }
        
        /// <summary>
        /// 根据条件值获取对应档位对象，获取权重ID对应的含义数值
        /// </summary>
        /// <param name="array">数值对象列表</param>
        /// <param name="conditionFun">数值对象的参数变量</param>
        /// <param name="conditionArrayFun">数值对象的权重列表</param>
        /// <param name="ratioArray">权重含义列表</param>
        /// <param name="condition">参数条件</param>
        /// <param name="name">log名字</param>
        /// <typeparam name="T">自定义数值对象</typeparam>
        /// <returns>合适档位对应权重</returns>
        public static int GetRandomByWeight<T>(T[] array, WeightD<T> conditionFun, WeightArray<T> conditionArrayFun, int[] ratioArray,double condition, string name)
        {
            var bean = GetSuitData(array, conditionFun, condition, name);
            return GetRandomByWeight(conditionArrayFun(bean), ratioArray, name);
        }
        
        /// <summary>
        /// 根据条件值获取对应档位对象，获取权重ID对应的含义数值
        /// </summary>
        /// <param name="array">数值对象列表</param>
        /// <param name="conditionFun">数值对象的参数变量</param>
        /// <param name="conditionArrayFun">数值对象的权重列表</param>
        /// <param name="ratioArray">权重含义列表</param>
        /// <param name="condition">参数条件</param>
        /// <param name="name">log名字</param>
        /// <typeparam name="T">自定义数值对象</typeparam>
        /// <returns>合适档位对应权重</returns>
        public static double GetRandomByWeight<T>(T[] array, WeightD<T> conditionFun, WeightArray<T> conditionArrayFun, double[] ratioArray,double condition, string name)
        {
            var bean = GetSuitData(array, conditionFun, condition, name);
            return GetRandomByWeight(conditionArrayFun(bean), ratioArray, name);
        }
        
        /// <summary>
        /// 获取权重ID对应的含义数值
        /// </summary>
        /// <param name="weightArray">权重列表</param>
        /// <param name="ratioArray">权重对应的含义列表</param>
        /// <param name="name">log名字</param>
        /// <returns>获取权重ID对应的含义数值</returns>
        public static int GetRandomByWeight(int[] weightArray, int[] ratioArray, string name)
        {
            var index = RandomWeightIndex(weightArray, name);
            var target = ratioArray[index];
            return target;
        }
        
        /// </summary>
        /// <param name="weightArray">权重列表</param>
        /// <param name="ratioArray">权重对应的含义列表</param>
        /// <param name="name">log名字</param>
        /// <returns>获取权重ID对应的含义数值</returns>
        public static double GetRandomByWeight(int[] weightArray, double[] ratioArray, string name)
        {
            var index = RandomWeightIndex(weightArray, name);
            var target = ratioArray[index];
            return target;
        }
        
        /// <summary>
        /// 根据权重列表计算合适对象
        /// </summary>
        /// <param name="weightList">权重列表</param>
        /// <param name="weight">对象的权重变量</param>
        /// <param name="name">log名字</param>
        /// <typeparam name="T">自定义对象</typeparam>
        /// <returns>合适对象</returns>
        public static T GetRandomByWeight<T>(T[] weightList, Weight<T> weight, string name)
        {
            var totalWeight = 0;
            foreach (var t in weightList)
            {
                var s = weight(t);
                totalWeight += s;
            }
            var target = default(T);
            if (totalWeight <= 0)
            {
                return target;
            }            
            var randomWeight = Random.Range(1, totalWeight+1); //随机范围[1,totalWeight]
            var total = 0;
            foreach (var entry in weightList)
            {
                total += weight(entry);
                if (total >= randomWeight)
                {
                    target = entry;
                    break;
                }
            }
            return target;
        }
        
        /// <summary>
        /// 根据权重列表计算合适对象
        /// </summary>
        /// <param name="weightList">权重列表</param>
        /// <param name="weight">对象的权重变量</param>
        /// <param name="name">log名字</param>
        /// <typeparam name="T">自定义对象</typeparam>
        /// <returns>合适对象</returns>
        public static T GetRandomByWeight<T>(List<T> weightList, Weight<T> weight, string name)
        {
            var totalWeight = 0;
            foreach (var t in weightList)
            {
                var s = weight(t);
                totalWeight += s;
            }
            var target = default(T);
            if (totalWeight <= 0)
            {
                return target;
            }            
            var randomWeight = Random.Range(1, totalWeight+1); //随机范围[1,totalWeight]
            var total = 0;
            foreach (var entry in weightList)
            {
                total += weight(entry);
                if (total >= randomWeight)
                {
                    target = entry;
                    break;
                }
            }
            return target;
        }

        /// <summary>
        /// 根据列表随机一个对象
        /// </summary>
        /// <param name="cards">对象列表</param>
        /// <typeparam name="T">自定义对象</typeparam>
        /// <returns>随机对象</returns>
        public static T[] Shuffle<T>(T[] cards)
        {
            for (var i = cards.Length - 1; i >= 0; i--)
            {
                var cardIndex = Random.Range(0, i);
                var temp = cards[cardIndex];
                cards[cardIndex] = cards[i];
                cards[i] = temp;
            }

            return cards;
        }
        
        /// <summary>
        /// 根据列表随机一个对象
        /// </summary>
        /// <param name="cards">对象列表</param>
        /// <typeparam name="T">自定义对象</typeparam>
        /// <returns>随机对象</returns>
        public static List<T> Shuffle<T>(List<T> cards)
        {
            for (var i = cards.Count - 1; i >= 0; i--)
            {
                var cardIndex = Random.Range(0, i);
                var temp = cards[cardIndex];
                cards[cardIndex] = cards[i];
                cards[i] = temp;
            }

            return cards;
        }

        /// <summary>
        /// 根据中奖几率是否中奖 分母10000
        /// </summary>
        /// <param name="prob"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        public static bool GetRandomProb(int prob,string key = null) {
            const int totalProb = 10000;
            var rand = (int) Random.Range(1,totalProb);
            return rand <= prob;
        }
    }
}
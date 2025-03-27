using Base.Core;
using StrategyDemo.Entity_NS;
using System;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.EventSystems.EventTrigger;

namespace Base.Pooling_NS
{
    public class BaseObjectPooler : Singleton<BaseObjectPooler>
    {
        private Dictionary<Type, Queue<MonoBehaviour>> pools = new Dictionary<Type, Queue<MonoBehaviour>>();

        public T GetObject<T>(T prefab) where T : MonoBehaviour
        {
            Type type = typeof(T);

            if (!pools.ContainsKey(type))
            {
                pools[type] = new Queue<MonoBehaviour>();
            }

            if (pools[type].Count > 0)
            {
                T obj = (T)pools[type].Dequeue();
                obj.gameObject.SetActive(true);
                return obj;
            }
            return Instantiate(prefab);
        }

        public void ReturnObject<T>(T obj) where T : BasePlaceableEntityController
        {
            Type type = typeof(T);

            if (!pools.ContainsKey(type))
            {
                pools[type] = new Queue<MonoBehaviour>();
            }

            obj.gameObject.SetActive(false);
            pools[type].Enqueue(obj);
        }
    }
}
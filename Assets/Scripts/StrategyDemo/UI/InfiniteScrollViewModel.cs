using StrategyDemo.Entity_NS;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Base.UI
{
    public class InfiniteScrollViewModel : MonoBehaviour
    {
        public List<SO_EntityData> itemDataList;
        public EntityButonView itemViewPrefab;

        public List<EntityButonView> _instantiatedItems = new (); //refactor

        public void InstatiateItem(Transform parent)
        {
            _instantiatedItems.Add(Instantiate(itemViewPrefab,parent));
            _instantiatedItems[_instantiatedItems.Count - 1].UpdateView(itemDataList[_instantiatedItems.Count - 1]);
        }

        public Transform GetLastItemTransform(int dataIndex)
        {
            EntityButonView item = GetItem(_instantiatedItems.Last(), dataIndex);
            _instantiatedItems.Insert(0, item);
            return item.transform;
        }

        public Transform GetFirstItemTransform(int dataIndex)
        {
            EntityButonView item = GetItem(_instantiatedItems.First(), dataIndex);
            _instantiatedItems.Add(item);
            return item.transform;
        }

        private EntityButonView GetItem(EntityButonView item, int dataIndex)
        {
            _instantiatedItems.Remove(item);
            item.UpdateView(itemDataList[((dataIndex % itemDataList.Count) + itemDataList.Count) % itemDataList.Count]);
            return item;
        }
    }
}
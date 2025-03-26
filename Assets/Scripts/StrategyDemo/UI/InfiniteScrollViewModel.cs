using StrategyDemo.Entity_NS;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Base.UI
{
    public class InfiniteScrollViewModel : MonoBehaviour
    {
        public List<SO_BaseEntityData> itemDataList;
        public EntityButonView itemViewPrefab;

        public List<EntityButonView> _instantiatedItems = new(); //refactor
        private float _itemHeight;
        private int _itemCountInARow = 0;

        public void InstatiateItems(int itemCount, Transform parent)
        {
            int instantiatedItemCount = _instantiatedItems.Count;
            _instantiatedItems.ForEach(x => x.gameObject.SetActive(false));
            for (int i = 0; i <= itemCount; i++)
            {
                if (instantiatedItemCount > 0 && i < instantiatedItemCount)
                {
                    _instantiatedItems[i].gameObject.SetActive(true);
                }
                else
                {
                    _instantiatedItems.Add(Instantiate(itemViewPrefab, parent));
                }
                _instantiatedItems[i].UpdateView(itemDataList[i]);
            }
        }

        public float GetItemHeight()
        {
            if(_itemHeight == 0)
            {
                _itemHeight = _instantiatedItems[_instantiatedItems.Count - 1].GetComponent<RectTransform>().sizeDelta.y;
            }
            return _itemHeight;
        }

        public int GetItemCountInRow()
        {
            if(_itemCountInARow == 0)
            {
                _itemCountInARow = 1;
                for (int i = 1; i <= _instantiatedItems.Count; i++)
                {
                    if (_instantiatedItems[i].GetComponent<RectTransform>().anchoredPosition.y == _instantiatedItems[i - 1].GetComponent<RectTransform>().anchoredPosition.y)
                    {
                        _itemCountInARow++;
                    }
                    else
                    {
                        return _itemCountInARow;
                    }
                }
            }            
            return _itemCountInARow;
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
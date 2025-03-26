using Base.Util;
using StrategyDemo.Entity_NS;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

namespace Base.UI
{
    public class InfiniteScrollViewController : MonoBehaviour
    {
        [SerializeField] private InfiniteScrollViewModel _model; //SO
        [SerializeField] private ScrollRect _scrollRect;

        private float _itemGap;

        private bool _firstItemReused;

        private int _maxScrollItemsSize = 24;

        private int _firstDataIndex;
        private int _lastDataIndex;

        private Vector2 _lastScrollPos = Vector2.one;
        private bool _scrollingTowardsLastItems = true;

        public void LoadScrollView(List<SO_BaseEntityData> itemDataList)
        {
            _scrollRect.movementType = ScrollRect.MovementType.Clamped;

            LayoutGroup layout = _scrollRect.content.GetComponent<LayoutGroup>();

            //Could Add Horizantal
            if (layout is GridLayoutGroup)
            {
                _itemGap = (layout as GridLayoutGroup).spacing.y;
            }
            else if (layout is VerticalLayoutGroup)
            {
                _itemGap = (layout as VerticalLayoutGroup).spacing;
            }

            _scrollRect.onValueChanged.RemoveAllListeners();
            if (IsPoolingNeeded())
                _scrollRect.onValueChanged.AddListener(OnScroll);

            _model.itemDataList.Clear();
            _model.itemDataList = itemDataList;
            _lastDataIndex = Mathf.Min(_maxScrollItemsSize - 1, _model.itemDataList.Count - 1);
            LoadItems();
        }

        private void OnDestroy()
        {
            _scrollRect.onValueChanged.RemoveAllListeners();
        }

        private bool IsPoolingNeeded()
        {
            return (_lastDataIndex < _model.itemDataList.Count - 1);
        }

        private void LoadItems()
        {
            _model.InstatiateItems(_lastDataIndex, _scrollRect.content.transform);
        }

        private void OnScroll(Vector2 scrollPos)
        {
            _scrollingTowardsLastItems = (scrollPos.x < _lastScrollPos.x || scrollPos.y < _lastScrollPos.y);
            _lastScrollPos = scrollPos;
            if (_scrollingTowardsLastItems && IsFirstItemAvailableToReuse())
            {
                ReuseFirstItem();
                if (!_firstItemReused)
                {
                    _firstItemReused = true;
                    _scrollRect.movementType = ScrollRect.MovementType.Unrestricted;
                }
            }
            else if (!_scrollingTowardsLastItems && IsLastItemAvailableToReuse() && _firstItemReused)
            {
                ReuseLastItem();
            }
        }

        private bool IsFirstItemAvailableToReuse()
        {
            return (_model._instantiatedItems.First().rect.transform.position.y > Screen.height * 1.5f);
        }

        private bool IsLastItemAvailableToReuse()
        {
            return (_model._instantiatedItems.Last().rect.transform.position.y > -Screen.height * 2f && _model._instantiatedItems.Last().rect.transform.position.y < -Screen.height * 1.5f);
        }

        private void ReuseFirstItem() //Refactor
        {
            _firstDataIndex++;
            _lastDataIndex++;
            RectTransform rectTransform = _model.GetFirstItemTransform(_lastDataIndex).GetComponent<RectTransform>();
            rectTransform.anchoredPosition = new Vector2(rectTransform.anchoredPosition.x, (-(_lastDataIndex / _model.GetItemCountInRow()) * (_model.GetItemHeight() + _itemGap)));
        }

        private void ReuseLastItem()
        {
            _firstDataIndex--;
            _lastDataIndex--;
            RectTransform rectTransform = _model.GetLastItemTransform(_firstDataIndex).GetComponent<RectTransform>();
            rectTransform.anchoredPosition = new Vector2(rectTransform.anchoredPosition.x, (-(_firstDataIndex / _model.GetItemCountInRow()) * (_model.GetItemHeight() + _itemGap)));

        }

        private void OnValidate()
        {
            if (!_model)
            {
                ObjectFinder.FindObjectInChilderenWithType(ref _model, transform);
            }
            if (!_scrollRect)
            {
                ObjectFinder.FindObjectInChilderenWithType(ref _scrollRect, transform);
            }
        }
    }
}
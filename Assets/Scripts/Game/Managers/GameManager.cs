using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using Game.Logic;
using UnityEngine;

namespace Game.Managers
{
    public class GameManager : Singleton<GameManager>
    {
        [SerializeField] private DraggableItem draggableItemPrefab;

        private readonly List<DraggableItem> _items = new();

        private List<DraggableItem> _lastDraggingItems;
        private Vector3 _lastItemPosition;
        private bool _isNew;

        private void Start()
        {
            var itemDataList = SaveManager.Instance.LoadItems();
            foreach (var itemData in itemDataList)
            {
                var item = CreateDraggableItem(itemData);
                _items.Add(item);
            }

            return;


            DraggableItem CreateDraggableItem(DraggableItemData data)
            {
                var lastRectTransform = _items.Count > 0 ? _items.Last().RectTransform : UIManager.Instance.Overlay;

                var item = Instantiate(draggableItemPrefab, lastRectTransform, false);
                item.RectTransform.anchoredPosition = data.position;
                item.Init(data.color);
                return item;
            }
        }

        private void OnApplicationQuit()
        {
            SaveManager.Instance.SaveItems(_items);
        }

        public void StartDrag(DraggableItem item)
        {
            UIManager.Instance.SetStatus("drag");

            item.RectTransform.SetParent(UIManager.Instance.Overlay);

            _isNew = !_items.Contains(item);
            if (!_isNew)
            {
                var itemIndex = _items.IndexOf(item);
                _lastDraggingItems = _items.GetRange(itemIndex, _items.Count - itemIndex);
                _items.RemoveRange(itemIndex, _items.Count - itemIndex);

                _lastItemPosition = item.RectTransform.anchoredPosition;
            }
        }

        public void EndDrag(DraggableItem item)
        {
            if (CanDropOnBoard(item))
            {
                UIManager.Instance.SetStatus("drop_on_board");
                DropOnBoard(item, true);
                ResetState();
                return;
            }

            if (CanDropInHole(item))
            {
                UIManager.Instance.SetStatus("drop_in_hole");
                DropInHole(item);
                ResetState();
                return;
            }

            if (!_isNew)
            {
                UIManager.Instance.SetStatus("drop_back");
                DropOnBoard(item, false);
                ResetState();
                return;
            }

            UIManager.Instance.SetStatus("destroy");
            DestroyItem(item, true);
            ResetState();
        }

        private bool CanDropOnBoard(DraggableItem item)
        {
            if (!IsItemInside(item, UIManager.Instance.RightPart))
                return false;

            if (_items.Count > 0)
            {
                var lastRectTransform = _items.Last().RectTransform;

                if (item.RectTransform.position.y < lastRectTransform.position.y + lastRectTransform.rect.height / 2)
                {
                    return false;
                }

                if (item.RectTransform.position.x < lastRectTransform.position.x - lastRectTransform.rect.width / 2 ||
                    item.RectTransform.position.x > lastRectTransform.position.x + lastRectTransform.rect.width / 2)
                {
                    return false;
                }
            }

            return true;
        }

        private bool IsItemInside(DraggableItem item, RectTransform targetRectTransform)
        {
            return RectTransformUtility.RectangleContainsScreenPoint(
                targetRectTransform,
                item.RectTransform.position,
                null,
                item.Offsets);
        }

        private void DropOnBoard(DraggableItem item, bool useTargetPosition)
        {
            if (_items.Count > 0)
            {
                var lastRectTransform = _items.Last().RectTransform;

                item.RectTransform.SetParent(lastRectTransform);
                if (useTargetPosition)
                {
                    var targetPosition = new Vector2(
                        lastRectTransform.InverseTransformPoint(item.RectTransform.position).x,
                        lastRectTransform.rect.height / 2 + item.RectTransform.rect.height / 2);
                    item.RectTransform.DOLocalMove(targetPosition, 0.2f);
                }
                else
                {
                    item.RectTransform.DOAnchorPos(_lastItemPosition, 0.2f);
                }
            }
            else if (!useTargetPosition)
            {
                item.RectTransform.DOAnchorPos(_lastItemPosition, 0.2f);
            }

            if (_lastDraggingItems != null)
            {
                _items.AddRange(_lastDraggingItems);
            }
            else
            {
                _items.Add(item);
            }
        }

        private bool CanDropInHole(DraggableItem item)
        {
            if (!IsObjectInsideHole(UIManager.Instance.HoleRectTransform, item.RectTransform))
                return false;

            return true;
        }

        private static bool IsObjectInsideHole(RectTransform holeRectTransform, RectTransform rectTransform)
        {
            Vector2 localPosition = holeRectTransform.InverseTransformPoint(rectTransform.position);

            // Уравнение эллипса: (x/a)^2 + (y/b)^2 <= 1
            var normalizedX = localPosition.x / (holeRectTransform.rect.width / 2);
            var normalizedY = localPosition.y / (holeRectTransform.rect.height / 2);
            return (normalizedX * normalizedX + normalizedY * normalizedY) <= 1;
        }

        private void DropInHole(DraggableItem item)
        {
            if (_isNew)
            {
                SetAnimation(item, 0);
            }
            else
            {
                for (var i = _lastDraggingItems.Count - 1; i >= 0; i--)
                {
                    _lastDraggingItems[i].RectTransform.SetParent(UIManager.Instance.Overlay);
                    _lastDraggingItems[i].RectTransform.SetAsLastSibling();
                    SetAnimation(_lastDraggingItems[i], 0.15f * (_lastDraggingItems.Count - 1 - i));
                }
            }

            return;


            void SetAnimation(DraggableItem tempItem, float delay)
            {
                var targetPosition = UIManager.Instance.HoleRectTransform.position;
                DOTween.Sequence()
                    .Join(tempItem.RectTransform.DOScale(1.1f, 0.1f))
                    .Append(tempItem.RectTransform.DOScale(0f, 0.4f))
                    .Insert(0, tempItem.RectTransform.DOJump(targetPosition, 150f, 1, 0.5f))
                    .SetDelay(delay)
                    .OnComplete(() => DestroyItem(tempItem, false));
            }
        }

        private void DestroyItem(DraggableItem item, bool withAnimation)
        {
            if (withAnimation)
            {
                item.RectTransform.DOScale(0, 0.4f).OnComplete(() => Destroy(item.gameObject));
            }
            else
            {
                Destroy(item.gameObject);
            }
        }

        private void ResetState()
        {
            _lastDraggingItems = null;
            _lastItemPosition = Vector3.zero;
        }
    }
}
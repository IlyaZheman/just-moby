﻿using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using Game.Logic;
using UnityEngine;

namespace Game.Managers
{
    public class GameManager : Singleton<GameManager>
    {
        [SerializeField] private DraggableItem draggableItemPrefab;

        private List<DraggableItem> _items = new();

        private List<DraggableItem> _lastDraggingItems = null;
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
                item.transform.position = data.position;
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

                _lastItemPosition = item.RectTransform.position;
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
            DestroyItem(item);
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
                        item.RectTransform.position.x,
                        lastRectTransform.position.y + lastRectTransform.rect.height / 2 +
                        item.RectTransform.rect.height / 2);
                    item.RectTransform.DOMove(targetPosition, 0.2f);
                }
                else
                {
                    item.RectTransform.DOMove(_lastItemPosition, 0.2f);
                }
            }
            else if (!useTargetPosition)
            {
                item.RectTransform.DOMove(_lastItemPosition, 0.2f);
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
            //
        }

        private void DestroyItem(DraggableItem item)
        {
            Destroy(item.gameObject);
        }

        private void ResetState()
        {
            _lastDraggingItems = null;
            _lastItemPosition = Vector3.zero;
        }
    }
}
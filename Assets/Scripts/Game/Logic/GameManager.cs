using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Game.Logic
{
    public class GameManager : Singleton<GameManager>
    {
        private List<DraggableItem> _items = new();
        private List<DraggableItem> _lastDraggingItems = null;

        public void StartDrag(DraggableItem item)
        {
            UIManager.Instance.SetStatus("Start Drag");

            item.RectTransform.SetParent(UIManager.Instance.Overlay);

            if (_items.Contains(item))
            {
                var itemIndex = _items.IndexOf(item);
                _lastDraggingItems = _items.GetRange(itemIndex, _items.Count - itemIndex);
                _items.RemoveRange(itemIndex, _items.Count - itemIndex);
            }
        }

        public void EndDrag(DraggableItem item)
        {
            UIManager.Instance.SetStatus("End Drag");

            if (CanDropOnAnotherItem(item))
            {
                if (_items.Count > 0)
                {
                    var lastRectTransform = _items.Last().RectTransform;

                    item.RectTransform.SetParent(lastRectTransform);
                    item.RectTransform.position = new Vector2(
                        item.RectTransform.position.x,
                        lastRectTransform.position.y + lastRectTransform.rect.height / 2 +
                        item.RectTransform.rect.height / 2);
                }

                if (_lastDraggingItems != null)
                {
                    _items.AddRange(_lastDraggingItems);
                }
                else
                {
                    _items.Add(item);
                }

                return;
            }
            else if (CanDropInHole(item))
            {
                return;
            }

            _lastDraggingItems = null;
            Destroy(item.gameObject);
        }

        public bool CanDropOnAnotherItem(DraggableItem item)
        {
            if (item.RectTransform.position.y + item.RectTransform.rect.height / 2 > Screen.height)
                return false;

            if (_items.Count > 0)
            {
                var lastRectTransform = _items.Last().RectTransform;

                if (item.RectTransform.position.y - item.RectTransform.rect.height / 2 <
                    lastRectTransform.position.y + lastRectTransform.rect.height / 2)
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

        public bool CanDropInHole(DraggableItem item)
        {
            return false;
        }
    }
}
using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Game.Logic
{
    public class StaticItem : Item,
        IPointerDownHandler, IPointerUpHandler,
        IDragHandler, IBeginDragHandler, IEndDragHandler
    {
        [SerializeField] private DraggableItem itemPrefab;

        private ScrollRect _scrollRect;
        private IEnumerator _checkCoroutine;

        private bool _isDraggingSlot;

        public void SetScrollRect(ScrollRect scrollRect)
        {
            _scrollRect = scrollRect;
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            _checkCoroutine = CheckDrag(eventData);
            StartCoroutine(_checkCoroutine);
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            if (_checkCoroutine != null)
            {
                StopCoroutine(_checkCoroutine);
            }
        }

        private IEnumerator CheckDrag(PointerEventData eventData)
        {
            while (true)
            {
                if (eventData.delta.y > Mathf.Abs(eventData.delta.x))
                {
                    _isDraggingSlot = true;

                    ExecuteEvents.Execute(gameObject, eventData, ExecuteEvents.endDragHandler);

                    var item = Instantiate(itemPrefab, transform.parent.parent.parent, false);
                    item.RectTransform.position = transform.position;
                    item.Init(Color);

                    eventData.pointerDrag = item.gameObject;

                    break;
                }

                yield return null;
            }
        }

        public void OnBeginDrag(PointerEventData eventData)
        {
            ExecuteEvents.Execute(_scrollRect.gameObject, eventData, ExecuteEvents.beginDragHandler);
        }

        public void OnDrag(PointerEventData eventData)
        {
            if (!_isDraggingSlot)
            {
                ExecuteEvents.Execute(_scrollRect.gameObject, eventData, ExecuteEvents.dragHandler);
            }
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            ExecuteEvents.Execute(_scrollRect.gameObject, eventData, ExecuteEvents.endDragHandler);

            if (_isDraggingSlot)
            {
                _isDraggingSlot = false;
            }
        }
    }
}
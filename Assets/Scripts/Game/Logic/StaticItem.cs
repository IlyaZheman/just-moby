using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Game.Logic
{
    public class StaticItem : Item,
        IPointerDownHandler, IPointerUpHandler,
        IDragHandler, IBeginDragHandler, IEndDragHandler
    {
        private ScrollRect _scrollRect;
        private IEnumerator _checkCoroutine;

        private bool _isDraggingSlot;

        private List<Vector2> _deltas = new();

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
            _deltas = new List<Vector2>(3);

            while (true)
            {
                if (_deltas.Count >= 3)
                {
                    if (_deltas.TrueForAll(delta => Vector2.Angle(delta, Vector2.up) > GameManager.BeginDragAngle / 2f))
                        break;

                    _isDraggingSlot = true;

                    ExecuteEvents.Execute(gameObject, eventData, ExecuteEvents.endDragHandler);

                    var item = Instantiate(GameManager.DraggableItemPrefab, transform.parent.parent.parent);
                    item.RectTransform.position = transform.position;
                    item.Init(Color);

                    eventData.pointerDrag = item.gameObject;
                    ExecuteEvents.Execute(eventData.pointerDrag, eventData, ExecuteEvents.beginDragHandler);
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
                _deltas.Add(eventData.delta);

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
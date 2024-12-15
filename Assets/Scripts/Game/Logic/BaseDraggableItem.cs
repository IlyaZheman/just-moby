using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Game.Logic
{
    public class BaseDraggableItem : MonoBehaviour,
        IPointerDownHandler, IPointerUpHandler, IPointerExitHandler,
        IDragHandler, IBeginDragHandler, IEndDragHandler
    {
        [SerializeField] private Image image;

        private ScrollRect _scrollRect;
        private IEnumerator _checkCoroutine;
        private bool _isDraggingSlot;

        public bool Draggable { get; set; } = true;

        public void OnPointerDown(PointerEventData eventData)
        {
            if (!Draggable)
                return;

            _checkCoroutine = CheckDrag(eventData);
            StartCoroutine(_checkCoroutine);
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            if (_checkCoroutine != null)
            {
                StopCoroutine(_checkCoroutine);
            }
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
                if (eventData.delta.y > 0)
                {
                    _isDraggingSlot = true;
                    OnBeginRealDrag(eventData);
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
            if (_isDraggingSlot)
            {
                OnRealDrag(eventData);
            }
            else
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
                OnEndRealDrag(eventData);
            }
        }

        public virtual void Init(ScrollRect scrollRect, Color color)
        {
            _scrollRect = scrollRect;
            image.color = color;
        }

        protected virtual void OnBeginRealDrag(PointerEventData eventData)
        {
        }

        protected virtual void OnRealDrag(PointerEventData eventData)
        {
        }

        protected virtual void OnEndRealDrag(PointerEventData eventData)
        {
        }
    }
}
using UnityEngine;
using UnityEngine.EventSystems;

namespace Game.Logic
{
    public class DraggableItem : BaseDraggableItem
    {
        private RectTransform _rectTransform;
        private Canvas _canvas;

        private void Awake()
        {
            _rectTransform = GetComponent<RectTransform>();
        }

        protected override void OnBeginRealDrag(PointerEventData eventData)
        {
            _canvas = transform.parent.GetComponentInParent<Canvas>();
            transform.SetParent(_canvas.transform);
        }

        protected override void OnRealDrag(PointerEventData eventData)
        {
            _rectTransform.anchoredPosition += eventData.delta / _canvas.scaleFactor;
        }

        protected override void OnEndRealDrag(PointerEventData eventData)
        {
        }
    }
}
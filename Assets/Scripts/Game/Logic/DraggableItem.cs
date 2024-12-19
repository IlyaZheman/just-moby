using UnityEngine.EventSystems;

namespace Game.Logic
{
    public class DraggableItem : Item, IBeginDragHandler, IDragHandler, IEndDragHandler
    {
        private bool _isDragging;

        public void OnBeginDrag(PointerEventData eventData)
        {
            _isDragging = ItemsManager.CanDrag(this);

            if (!_isDragging)
                return;

            UpdateCanvas();

            ItemsManager.StartDrag(this);
        }

        public void OnDrag(PointerEventData eventData)
        {
            if (!_isDragging)
                return;

            RectTransform.anchoredPosition += eventData.delta / Canvas.scaleFactor;
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            if (!_isDragging)
                return;

            ItemsManager.EndDrag(this);
        }
    }
}
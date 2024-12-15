using UnityEngine.EventSystems;

namespace Game.Logic
{
    public class DraggableItem : Item, IBeginDragHandler, IDragHandler, IEndDragHandler
    {
        public void OnBeginDrag(PointerEventData eventData)
        {
            UpdateCanvas();
            RectTransform.SetParent(Canvas.transform);
        }

        public void OnDrag(PointerEventData eventData)
        {
            RectTransform.anchoredPosition += eventData.delta / Canvas.scaleFactor;
        }

        public void OnEndDrag(PointerEventData eventData)
        {
        }
    }
}
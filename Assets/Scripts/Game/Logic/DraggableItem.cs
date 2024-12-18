using Game.Managers;
using UnityEngine.EventSystems;

namespace Game.Logic
{
    public class DraggableItem : Item, IBeginDragHandler, IDragHandler, IEndDragHandler
    {
        public void OnBeginDrag(PointerEventData eventData)
        {
            UpdateCanvas();

            ItemsManager.StartDrag(this);
        }

        public void OnDrag(PointerEventData eventData)
        {
            RectTransform.anchoredPosition += eventData.delta / Canvas.scaleFactor;
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            ItemsManager.EndDrag(this);
        }
    }
}
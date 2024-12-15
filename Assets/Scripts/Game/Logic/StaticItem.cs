using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Game.Logic
{
    public class StaticItem : Item, IPointerDownHandler, IPointerUpHandler
    {
        [SerializeField] private DraggableItem itemPrefab;

        private IEnumerator _checkCoroutine;

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
                    var item = Instantiate(itemPrefab, transform.parent.parent.parent, false);
                    item.RectTransform.position = transform.position;
                    item.Init(Image.color);

                    eventData.pointerDrag = item.gameObject;

                    break;
                }

                yield return null;
            }
        }
    }
}
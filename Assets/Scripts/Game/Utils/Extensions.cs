using Game.Logic;
using UnityEngine;

namespace Game.Utils
{
    public static partial class Extensions
    {
        public static bool IsItemInside(this Item item, RectTransform targetRectTransform)
        {
            return RectTransformUtility.RectangleContainsScreenPoint(
                targetRectTransform,
                item.RectTransform.position,
                null,
                item.Offsets);
        }

        public static bool IsObjectInsideHole(this RectTransform itemRectTransform, RectTransform holeRectTransform)
        {
            Vector2 localPosition = holeRectTransform.InverseTransformPoint(itemRectTransform.position);

            // Уравнение эллипса: (x/a)^2 + (y/b)^2 <= 1
            var normalizedX = localPosition.x / (holeRectTransform.rect.width / 2);
            var normalizedY = localPosition.y / (holeRectTransform.rect.height / 2);
            return normalizedX * normalizedX + normalizedY * normalizedY <= 1;
        }
    }
}
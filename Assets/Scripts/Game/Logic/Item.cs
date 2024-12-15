using UnityEngine;
using UnityEngine.UI;

namespace Game.Logic
{
    public class Item : MonoBehaviour
    {
        public RectTransform RectTransform { get; protected set; }
        public Image Image { get; protected set; }
        public Canvas Canvas { get; protected set; }

        private void Awake()
        {
            RectTransform = GetComponent<RectTransform>();
            Image = GetComponent<Image>();
            UpdateCanvas();
        }

        public void Init(Color color)
        {
            Image.color = color;
        }

        protected void UpdateCanvas()
        {
            Canvas = transform.parent.GetComponentInParent<Canvas>();
        }
    }
}
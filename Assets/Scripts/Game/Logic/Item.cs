using UnityEngine;
using UnityEngine.UI;

namespace Game.Logic
{
    public class Item : MonoBehaviour
    {
        public RectTransform RectTransform { get; protected set; }
        public Canvas Canvas { get; protected set; }
        public Vector4 Offsets { get; protected set; }

        private Image _image;
        public Color Color
        {
            get => _image.color;
            protected set => _image.color = value;
        }

        private void Awake()
        {
            RectTransform = GetComponent<RectTransform>();
            _image = GetComponent<Image>();
            UpdateCanvas();
            Offsets = new Vector4(
                RectTransform.rect.width / 2,
                RectTransform.rect.height / 2,
                RectTransform.rect.width / 2,
                RectTransform.rect.height / 2);
        }

        public void Init(Color color)
        {
            Color = color;
        }

        protected void UpdateCanvas()
        {
            Canvas = transform.parent.GetComponentInParent<Canvas>();
        }
    }
}
using Game.Managers;
using UnityEngine;
using UnityEngine.UI;

namespace Game.Logic
{
    public class Item : MonoBehaviour
    {
        public RectTransform RectTransform { get; protected set; }
        public Canvas Canvas { get; protected set; }
        public Vector4 Offsets { get; protected set; }
        public Image Image { get; protected set; }
        public Color Color => Image.color;

        protected GameManager GameManager;
        protected ItemsManager ItemsManager;

        private void Awake()
        {
            GameManager = GameManager.Instance;
            ItemsManager = ItemsManager.Instance;

            RectTransform = GetComponent<RectTransform>();
            Image = GetComponent<Image>();
            UpdateCanvas();
            Offsets = new Vector4(
                RectTransform.rect.width / 2,
                RectTransform.rect.height / 2,
                RectTransform.rect.width / 2,
                RectTransform.rect.height / 2);
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
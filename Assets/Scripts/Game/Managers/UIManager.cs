using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Game.Managers
{
    public class UIManager : Singleton<UIManager>
    {
        [Header("Right Part")]
        [SerializeField]
        private RectTransform rightPart;
        public RectTransform RightPart => rightPart;

        [Header("Left Part")]
        [SerializeField]
        private RectTransform holeRectTransform;
        public RectTransform HoleRectTransform => holeRectTransform;

        [Header("Bottom Part")]
        [SerializeField]
        private Transform staticItemsContainer;
        public Transform StaticItemsContainer => staticItemsContainer;

        [SerializeField]
        private ScrollRect scrollRect;
        public ScrollRect ScrollRect => scrollRect;

        [SerializeField]
        private TMP_Text statusText;
        public TMP_Text StatusText => statusText;

        [SerializeField]
        private string defaultStatusTextKey;
        public string DefaultStatusTextKey => defaultStatusTextKey;

        [Header("Overlay")]
        [SerializeField]
        private RectTransform overlay;
        public RectTransform Overlay => overlay;

        private LocalizationManager _localizationManager;

        protected override void Initialize()
        {
            _localizationManager = LocalizationManager.Instance;
        }

        protected override void StartManager()
        {
            StatusText.SetText(_localizationManager.GetLocalizationData(DefaultStatusTextKey));
        }

        public void SetStatus(string key)
        {
            var localizedText = _localizationManager.GetLocalizationData(key);
            statusText.SetText(localizedText);
        }
    }
}
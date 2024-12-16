using TMPro;
using UnityEngine;

namespace Game.Managers
{
    public class UIManager : Singleton<UIManager>
    {
        [Header("Right Part")]
        [SerializeField] private RectTransform rightPart;
        public RectTransform RightPart => rightPart;

        [Header("Left Part")]
        [SerializeField] private RectTransform holeRectTransform;
        public RectTransform HoleRectTransform => holeRectTransform;

        [Header("Bottom Part")]
        [SerializeField] private TMP_Text statusText;

        [Header("Overlay")]
        [SerializeField] private RectTransform overlay;
        public RectTransform Overlay => overlay;

        public void SetStatus(string key)
        {
            var localizedText = LocalizationManager.Instance.GetLocalizationData(key);
            statusText.SetText(localizedText);
        }
    }
}
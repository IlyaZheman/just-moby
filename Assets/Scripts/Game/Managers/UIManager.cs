using TMPro;
using UnityEngine;

namespace Game.Managers
{
    public class UIManager : Singleton<UIManager>
    {
        [SerializeField] private Transform overlay;
        [SerializeField] private TMP_Text statusText;

        public Transform Overlay => overlay;

        public void SetStatus(string key)
        {
            var localizedText = LocalizationManager.Instance.GetLocalizationData(key);
            statusText.SetText(localizedText);
        }
    }
}
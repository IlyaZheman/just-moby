using System.Linq;
using Game.Data;
using TMPro;
using UnityEngine;

namespace Game.Logic
{
    public class UIManager : Singleton<UIManager>
    {
        [SerializeField] private LocalizationDataStorage localizationDataStorage;

        [SerializeField] private Transform overlay;
        [SerializeField] private TMP_Text statusText;

        public Transform Overlay => overlay;

        public void SetStatus(string key)
        {
            var localizedText = GetLocalizationData(key);
            statusText.SetText(localizedText);
        }

        private string GetLocalizationData(string key)
        {
            foreach (var data in localizationDataStorage.Data)
            {
                if (data.Key == key)
                {
                    return data.Types.First(t => t.Language == GetSystemLanguage()).Text;
                }
            }

            return "<color=red>Unknown</color>";
        }

        private SystemLanguage GetSystemLanguage()
        {
            var language = localizationDataStorage.DefaultLanguage == SystemLanguage.Unknown
                ? Application.systemLanguage
                : localizationDataStorage.DefaultLanguage;

            switch (language)
            {
                case SystemLanguage.Russian:
                    return SystemLanguage.Russian;
                default:
                    return SystemLanguage.English;
            }
        }
    }
}
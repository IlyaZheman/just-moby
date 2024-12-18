using System.Linq;
using Game.Data;
using UnityEngine;

namespace Game.Managers
{
    public class LocalizationManager : Singleton<LocalizationManager>
    {
        [SerializeField]
        private LocalizationDataStorage localizationDataStorage;

        public string GetLocalizationData(string key)
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
using System;
using UnityEngine;

namespace Game.Data
{
    [CreateAssetMenu(fileName = "ds_localization", menuName = "Data/Localization", order = 0)]
    public class LocalizationDataStorage : ScriptableObject
    {
        [SerializeField] private SystemLanguage defaultLanguage;
        [SerializeField] private LocalizationData[] data;

        public SystemLanguage DefaultLanguage => defaultLanguage;
        public LocalizationData[] Data => data;
    }

    [Serializable]
    public class LocalizationData
    {
        [SerializeField] private string key;
        [SerializeField] private LocalizationType[] types;

        public string Key => key;
        public LocalizationType[] Types => types;
    }

    [Serializable]
    public class LocalizationType
    {
        [SerializeField] private SystemLanguage language;
        [SerializeField] private string text;

        public SystemLanguage Language => language;
        public string Text => text;
    }
}
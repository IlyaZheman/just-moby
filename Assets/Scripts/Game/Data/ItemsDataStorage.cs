using System;
using UnityEngine;

namespace Game.Data
{
    [CreateAssetMenu(fileName = "ds_items", menuName = "Data/Items", order = 0)]
    public class ItemsDataStorage : ScriptableObject
    {
        [SerializeField]
        private ItemData[] items;
        public ItemData[] Items => items;
    }

    [Serializable]
    public class ItemData
    {
        [SerializeField]
        private Color color;
        public Color Color => color;
    }
}
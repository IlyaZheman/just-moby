using System;
using System.Collections.Generic;
using System.IO;
using Game.Logic;
using UnityEngine;

namespace Game.Managers
{
    public class SaveManager : Singleton<SaveManager>
    {
        private string _saveFilePath;

        protected override void Initialize()
        {
            _saveFilePath = Application.persistentDataPath + "/savefile.json";
        }

        public void SaveItems(List<DraggableItem> items)
        {
            var itemDataList = new List<DraggableItemData>();
            foreach (var item in items)
            {
                itemDataList.Add(new DraggableItemData(item));
            }

            var json = JsonUtility.ToJson(new Serialization<DraggableItemData>(itemDataList), true);
            File.WriteAllText(_saveFilePath, json);
        }

        public List<DraggableItemData> LoadItems()
        {
            if (File.Exists(_saveFilePath))
            {
                var json = File.ReadAllText(_saveFilePath);
                return JsonUtility.FromJson<Serialization<DraggableItemData>>(json).ToList();
            }

            return new List<DraggableItemData>();
        }
    }

    [Serializable]
    public class DraggableItemData
    {
        public Vector2 position;
        public Color color;

        public DraggableItemData(DraggableItem item)
        {
            position = item.RectTransform.anchoredPosition;
            color = item.Color;
        }
    }

    [Serializable]
    public class Serialization<T>
    {
        [SerializeField]
        private List<T> target;

        public Serialization(List<T> target)
        {
            this.target = target;
        }

        public List<T> ToList()
        {
            return target;
        }
    }
}
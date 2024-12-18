using System.Collections.Generic;
using System.Linq;
using Game.Data;
using Game.Logic;
using UnityEngine;

namespace Game.Managers
{
    public class GameManager : Singleton<GameManager>
    {
        [SerializeField]
        private ItemsDataStorage itemsDataStorage;
        [SerializeField]
        private DraggableItem draggableItemPrefab;
        [SerializeField]
        private StaticItem staticItemPrefab;

        public DraggableItem DraggableItemPrefab => draggableItemPrefab;
        public StaticItem StaticItemPrefab => staticItemPrefab;

        public List<DraggableItem> Items { get; private set; } = new();

        private void Start()
        {
            SpawnStaticItems();
            SpawnDraggableItems();
        }

        private void SpawnStaticItems()
        {
            foreach (var itemData in itemsDataStorage.Items)
            {
                var item = Instantiate(StaticItemPrefab, UIManager.Instance.StaticItemsContainer);
                item.Init(itemData.Color);
                item.SetScrollRect(UIManager.Instance.ScrollRect);
            }
        }

        private void SpawnDraggableItems()
        {
            var itemDataList = SaveManager.Instance.LoadItems();
            foreach (var itemData in itemDataList)
            {
                var lastRectTransform = Items.Count > 0 ? Items.Last().RectTransform : UIManager.Instance.Overlay;

                var item = Instantiate(DraggableItemPrefab, lastRectTransform);
                item.RectTransform.anchoredPosition = itemData.position;
                item.Init(itemData.color);
                Items.Add(item);
            }
        }

        private void OnApplicationQuit()
        {
            SaveManager.Instance.SaveItems(Items);
        }
    }
}
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
        private float beginDragAngle;
        [SerializeField]
        private DraggableItem draggableItemPrefab;
        [SerializeField]
        private StaticItem staticItemPrefab;

        private UIManager _uiManager;
        private SaveManager _saveManager;

        public float BeginDragAngle => beginDragAngle;
        public DraggableItem DraggableItemPrefab => draggableItemPrefab;
        public StaticItem StaticItemPrefab => staticItemPrefab;

        public List<DraggableItem> Items { get; private set; } = new();

        protected override void Initialize()
        {
            _uiManager = UIManager.Instance;
            _saveManager = SaveManager.Instance;
        }

        protected override void StartManager()
        {
            SpawnStaticItems();
            SpawnDraggableItems();
        }

        private void SpawnStaticItems()
        {
            foreach (var itemData in itemsDataStorage.Items)
            {
                var item = Instantiate(StaticItemPrefab, _uiManager.StaticItemsContainer);
                item.Init(itemData.Color);
                item.SetScrollRect(_uiManager.ScrollRect);
            }
        }

        private void SpawnDraggableItems()
        {
            var itemDataList = _saveManager.LoadItems();
            foreach (var itemData in itemDataList)
            {
                var lastRectTransform = Items.Count > 0 ? Items.Last().RectTransform : _uiManager.Overlay;

                var item = Instantiate(DraggableItemPrefab, lastRectTransform);
                item.RectTransform.anchoredPosition = itemData.position;
                item.Init(itemData.color);
                Items.Add(item);
            }
        }

        private void OnApplicationQuit()
        {
            _saveManager.SaveItems(Items);
        }
    }
}
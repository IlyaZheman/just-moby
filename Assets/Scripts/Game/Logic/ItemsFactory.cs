using Game.Data;
using UnityEngine;
using UnityEngine.UI;

namespace Game.Logic
{
    public class ItemsFactory : MonoBehaviour
    {
        [SerializeField] private ItemsDataStorage itemsDataStorage;
        [SerializeField] private ScrollRect scrollRect;
        [SerializeField] private BaseDraggableItem draggableItemPrefab;
        [SerializeField] private Transform itemsContainer;

        private void Awake()
        {
            GenerateItems();
        }

        private void GenerateItems()
        {
            foreach (var itemData in itemsDataStorage.Items)
            {
                var item = Instantiate(draggableItemPrefab, itemsContainer, false);
                item.Init(scrollRect, itemData.Color);
            }
        }
    }
}
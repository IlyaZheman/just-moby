using Game.Data;
using UnityEngine;

namespace Game.Logic
{
    public class ItemsFactory : MonoBehaviour
    {
        [SerializeField] private ItemsDataStorage itemsDataStorage;
        [SerializeField] private StaticItem itemPrefab;
        [SerializeField] private Transform itemsContainer;

        private void Start()
        {
            GenerateItems();
        }

        private void GenerateItems()
        {
            foreach (var itemData in itemsDataStorage.Items)
            {
                var item = Instantiate(itemPrefab, itemsContainer, false);
                item.Init(itemData.Color);
            }
        }
    }
}
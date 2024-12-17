using Game.Data;
using UnityEngine;
using UnityEngine.UI;

namespace Game.Logic
{
    public class ItemsFactory : MonoBehaviour
    {
        [SerializeField] private ItemsDataStorage itemsDataStorage;
        [SerializeField] private StaticItem itemPrefab;
        [SerializeField] private Transform itemsContainer;
        [SerializeField] private ScrollRect itemsScrollView;

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
                item.SetScrollRect(itemsScrollView);
            }
        }
    }
}
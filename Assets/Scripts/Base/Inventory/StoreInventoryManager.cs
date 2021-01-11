using System.Collections.Generic;
using DSD.KernalTool;
using UnityEngine;
using UnityEngine.Serialization;

namespace Base.Inventory
{
    public class StoreInventoryManager : MonoBehaviour
    {
        public static StoreInventoryManager Instance;

        [FormerlySerializedAs("Store")] public Inventory store;
        public GameObject slotGrid;
        public Slot slotPrefab;

        // 保存进来的回合数，若当前回合数与之前进来的回合数不一样就刷新商店
        private int _lastRound;
        private List<Item> _items;
        private void Awake()
        {
            if (Instance != null)
                Destroy(this);
            else
                Instance = this;
            _items = (Resources.Load("Inventory/Inventories/ItemInventory_Store") as Inventory)?.itemList;
        }

        private void Start()
        {
            _lastRound = 0;
        }

        private void OnEnable()
        {
            if (_lastRound != GlobalManager.Instance.player.CurRound)
            {
                RefreshItem();
                _lastRound = GlobalManager.Instance.player.CurRound;
            }
        }

        /// <summary>
        /// 初始化背包，清除当前背包，从asset加载背包
        /// </summary>
        private void RefreshItem()
        {
            // 清除背包
            for (var i = 0; i < slotGrid.transform.childCount; i++)
            {
                Destroy(slotGrid.transform.GetChild(i).gameObject);
            }

            UpdateInventory();
        }
        public void RemoveItem(Item item)
        {
            if (_items.Contains(item))
                _items.Remove(item);
            else
                Debug.LogWarning($"m_Items不含{item},无法移除");
        }
        /// <summary>
        /// 从商品堆里抽6个商品摆放
        /// </summary>
        private void UpdateInventory()
        {
            store.itemList.Clear();

            var randomSequence = Widget.GetRandomSequence(_items.Count, 6);
            // 从asset里挑选六个生成
            for (var i = 0; i < 6; i++)
            {
                var newItem = Instantiate(slotPrefab, slotGrid.transform);
                newItem.slotItem = _items[randomSequence[i]];
                newItem.slotImage.sprite = _items[randomSequence[i]].itemImage;
                newItem.isSell = true;
                store.itemList.Add(newItem.slotItem);
            }
        }
    }
}
using GameBase.Template.GameBase.Table;
using Service.Core;
using System;
using System.Collections.Generic;
using System.Text;

namespace TestClient.TestClient.Item
{
    public class ItemManager
    {
        private Dictionary<int /*itemId*/, Item> _itemById = new Dictionary<int, Item>();
        public Dictionary<int, Item> ItemById { get { return _itemById; } }
        public void CreateItem(int parentItemId, int groupIndex, int itemType, int itemId, int itemLevel, long totalValue, int remainTime)
        {
            Item createItem = new Item();
            createItem.ParentItemId = parentItemId;
            createItem.GroupIndex = groupIndex;
            createItem.ItemType = itemType;
            createItem.ItemId = itemId;
            createItem.ItemLevel = itemLevel;
            createItem.TotalValue = totalValue;
            createItem.RemainTime = remainTime;
            _itemById.Add(createItem.ItemId, createItem);
        }

        public void Clear()
        {
            _itemById.Clear();
        }

        public void UpdateItem(int itemId, long value, long totalValue)
        {
            var itemData = DataTable<int, ItemListTable>.Instance.GetData(itemId);
            Item item = null;
            if (_itemById.ContainsKey(itemData.id) == false)
            {
                item = new Item();
                item.ItemType = itemData.itemType;
                item.ItemId = itemId;
                _itemById.Add(itemData.id, item);
            }
            _itemById[itemId].TotalValue = totalValue;
        }
    }
}

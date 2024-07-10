using System;
using System.Collections.Generic;
using System.Text;

namespace TestClient.TestClient.Item
{
    public class Item
    {
        private int _parentItemId;
        public int ParentItemId
        {
            get { return _parentItemId; }
            set { _parentItemId = value; }
        }
        private int _groupIndex;
        public int GroupIndex
        {
            get { return _groupIndex; } 
            set { _groupIndex = value;} 
        }
        private int _itemType;
        public int ItemType
        {
            get { return _itemType; }
            set { _itemType = value; }
        }
        private int _itemId;
        public int ItemId
        {
            get { return _itemId; }
            set { _itemId = value; }
        }
        private int _itemLevel;
        public int ItemLevel
        {
            get { return _itemLevel; }
            set { _itemLevel = value; }
        }
        private long _totalValue;
        public long TotalValue
        {
            get { return _totalValue; }
            set { _totalValue = value; }
        }
        private int _remainTime;
        public int RemainTime
        {
            get { return _remainTime; }
            set { _remainTime = value; }
        }
    }
}

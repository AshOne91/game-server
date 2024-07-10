using System;
using System.Collections.Generic;
using System.Text;

namespace TestClient.TestClient.Shop
{
    public class ShopProduct
    {
        private int _shopProductId;
        public int ShopProductId { get { return _shopProductId; } set { _shopProductId = value; } }
        private byte _buyCount;
        public byte BuyCount { get { return _buyCount; } set { _buyCount = value; } }
    }
    public class ProductStat
    {
        private int _statusType;
        public int StatusType { get { return _statusType;} set { _statusType = value; } }
        private int _remainEndTime;
        public int RemainingEndTime { get { return _remainEndTime; } set { _remainEndTime = value; } }
    }
    public class Shop
    {
        private int _shopId;
        public int ShopId
        {
            get { return _shopId; }
            set { _shopId = value; }
        }
        private int _resetRemainType;
        public int ResetRemainType
        {
            get { return _resetRemainType; }
            set { _resetRemainType = value; }
        }
        private List<ShopProduct> _products = new List<ShopProduct>();
        public List<ShopProduct> Products
        {
            get { return _products; }
            set {  _products = value; }
        }
        private byte _pointResetCount;
        public byte PointResetCount
        {
            get { return _pointResetCount; }
            set { _pointResetCount = value; }
        }
        private List<ProductStat> _productStats = new List<ProductStat>();
        public List<ProductStat> productStats
        {
            get { return _productStats; }
            set { _productStats = value; }
        }
    }
}

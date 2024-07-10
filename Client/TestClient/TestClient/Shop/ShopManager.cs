using GameBase.Template.Shop.GameBaseShop.Common;
using System;
using System.Collections.Generic;
using System.Text;
using TestClient.FrameWork;

namespace TestClient.TestClient.Shop
{
    public class ShopManager
    {
        private Dictionary<int/*shopId*/, Shop> _shopByShopId = new Dictionary<int, Shop>();
        public Dictionary<int/*shopId*/, Shop> ShopByShopId { get { return _shopByShopId; } }

        public void Clear()
        {
            _shopByShopId.Clear();
        }

        public void CreateShop(ShopInfo shopInfo) 
        {
            Shop shop = new Shop();
            shop.ShopId = shopInfo.shopId;
            shop.ResetRemainType = shopInfo.resetRemainType;
            foreach (var productInfo in shopInfo.listProductInfo)
            {
                ShopProduct product = new ShopProduct();
                product.ShopProductId = productInfo.shopProductId;
                product.BuyCount = productInfo.buyCount;
                shop.Products.Add(product);
            }
            shop.PointResetCount = shopInfo.pointResetCount;
            foreach (var productStat in shopInfo.ProductStatusList)
            {
                ProductStat stat = new ProductStat();
                stat.StatusType = productStat.statusType;
                stat.RemainingEndTime = productStat.remainEndTime;
                shop.productStats.Add(stat);
            }
            _shopByShopId.Add(shop.ShopId, shop);
        }

        public void UpdateShop(int shopId, int shopProductId, byte buyCount)
        {
            Shop shopInfo = null;
            if (_shopByShopId.ContainsKey(shopId) == false)
            {
                _shopByShopId.Add(shopId, new Shop());
            }
            shopInfo = _shopByShopId[shopId];
            var shopProduct = shopInfo.Products.Find(x => x.ShopProductId == shopProductId);
            if (shopProduct == null)
            {
                shopProduct = new ShopProduct();
                shopInfo.Products.Add(shopProduct);
            }
            shopProduct.ShopProductId = shopProductId;
            shopProduct.BuyCount = buyCount;
        }

        private int _cur = 0;
        public int Cur
        {
            get { return _cur; }
        }
        public void Seek(int off)
        {
            if (_shopByShopId.Count == 0)
            {
                _cur = 0;
                return;
            }

            int count = 0;
            foreach(var shop in _shopByShopId)
            {
                count += shop.Value.Products.Count;
            }
            _cur = _cur + off;
            _cur = Math.Min(_cur, count - 1);
            _cur = Math.Max(_cur, 0);
        }
        public ShopProduct GetSeekProduct()
        {
            if (_shopByShopId.Count == 0)
            {
                return null;
            }

            ShopProduct shopProduct = null;
            int count = _cur;
            foreach(var shop in _shopByShopId)
            {
                foreach(var product in shop.Value.Products)
                {
                    if(count == 0)
                    {
                        shopProduct = product;
                        break;
                    }
                    count--;
                }
            }
            return shopProduct;
        }
        public Shop GetSeekShop()
        {
            if (_shopByShopId.Count == 0)
            {
                return null;
            }

            Shop shopInfo = null;
            int count = _cur;
            foreach (var shop in _shopByShopId)
            {
                foreach (var product in shop.Value.Products)
                {
                    if (count == 0)
                    {
                        shopInfo = shop.Value;
                        break;
                    }
                    count--;
                }
            }
            return shopInfo;
        }
    }
}

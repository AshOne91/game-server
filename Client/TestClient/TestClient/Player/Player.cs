using System;
using System.Collections.Generic;
using System.Text;
using TestClient.TestClient.Item;
using TestClient.TestClient.Shop;

namespace TestClient.TestClient.Player
{
    public class Player 
    {
        private ItemManager _itemManager = null;
        public ItemManager ItemManager
        {
            get
            {
                if (_itemManager == null)
                {
                    _itemManager = new ItemManager();
                }
                return _itemManager;
            }
        }
        private ShopManager _shopManager = null;
        public ShopManager ShopManager
        {
            get
            {
                if (_shopManager == null) 
                {
                    _shopManager = new ShopManager();
                }
                return _shopManager;
            }
        }

        private ulong _playerDBKey = 0;
        public ulong PlayerDBKey
        {
            get { return _playerDBKey; }
            set { _playerDBKey = value; }
        }
        private string _siteUserId = string.Empty;
        public string SiteUserId
        {
            get { return _siteUserId; }
            set { _siteUserId = value; }
        }
        private string _playerName = string.Empty;
        public string PlayerName
        {
            get { return _playerName; }
            set { _playerName = value; }
        }
        private string _version = string.Empty;
        public string Version
        {
            get { return _version; }
            set { _version = value; }
        }
        private string _matchToken = string.Empty;
        public string MatchToken
        {
            get { return _matchToken; }
            set { _matchToken = value; }
        }
    }
}

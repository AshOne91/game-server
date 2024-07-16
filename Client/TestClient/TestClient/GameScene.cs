using GameBase.Template.Account.GameBaseAccount;
using GameBase.Template.Account.GameBaseAccount.Common;
using GameBase.Template.GameBase;
using GameBase.Template.GameBase.Common;
using GameBase.Template.GameBase.Table;
using GameBase.Template.Item.GameBaseItem.Common;
using GameBase.Template.Shop.GameBaseShop;
using GameBase.Template.Shop.GameBaseShop.Common;
using Service.Core;
using Service.Net;
using System;
using System.Collections.Generic;
using System.Data;
using System.Numerics;
using System.Runtime.CompilerServices;
using System.Text;
using TestClient.FrameWork;
using TestClient.TestClient.Player;
using TestClient.TestClient.Shop;

namespace TestClient.TestClient
{
    public class GameScene : AppBaseScene<GameScene>
    {
        enum GameMainPage
        {
            None,
            Entry,
            Connecting,
            ConnectError,
            Disconnect,
            AuthComplete,
            Player,
            Item
        }

        enum GameSubPage
        {
            None,
            PlayerList,
            PlayerCreate,
            PlayerSelect,
            ItemList,
            ShopInfo,
            ItemBuy
        }

        enum GameSequence
        {
            ConnectGameServer,
            PlayerListRequest,
            CreatePlayer,
            SelectPlayer,
            ItemInfo,
            ShopInfo
        }

        private GameMainPage _mainPage = GameMainPage.None;
        private GameMainPage MainPage
        {
            get
            {
                return MainPage;
            }
            set
            {
                if (_mainPage == value) return;
                OnLeaveUI(_mainPage);
                _mainPage = value;
                OnEnterUI(_mainPage);
            }
        }

        private GameUserObject _gameUser = null;
        public GameUserObject GameUser
        {
            get { return _gameUser; }
            private set { _gameUser = value; }
        }

        private void OnLeaveUI(GameMainPage gameMainPage)
        {
            InputManager.Instance.Clear();
        }

        private void OnEnterUI(GameMainPage gameMainPage)
        {
            switch (_mainPage) 
            {
                case GameMainPage.Entry:
                    {
                        InputManager.Instance.Clear();
                        InputManager.Instance.AddInputMap(0, ConsoleKey.D1, () =>
                        {
                            NetworkManager.Instance.GameConnect(NetworkManager.Instance.LoginAuthInfo.IP
                                , NetworkManager.Instance.LoginAuthInfo.Port);
                        });
                    }
                    break;
                case GameMainPage.AuthComplete:
                    {
                        InputManager.Instance.Clear();
                        InputManager.Instance.AddInputMap(0, ConsoleKey.D1, () =>
                        {
                            MainPage = GameMainPage.Player;
                        });
                    }
                    break;
                case GameMainPage.Player:
                    {
                        InputManager.Instance.Clear();
                        InputManager.Instance.AddInputMap(0, ConsoleKey.D1, () =>
                        {
                            PACKET_CG_PLAYERLIST_REQ sendData = new PACKET_CG_PLAYERLIST_REQ();
                            GameUser.GetSession().SendPacket(sendData.Serialize());
                        });
                        InputManager.Instance.AddInputMap(0, ConsoleKey.D2, () =>
                        {
                            PACKET_CG_CREATE_PLAYER_REQ sendData = new PACKET_CG_CREATE_PLAYER_REQ();
                            sendData.PlayerName = Guid.NewGuid().ToString();
                            GameUser.GetSession().SendPacket(sendData.Serialize());
                        });
                        InputManager.Instance.AddInputMap(0, ConsoleKey.D3, () =>
                        {
                            var player = PlayerManager.Instance.GetSeekPlayer();
                            if (player == null)
                            {
                                return;
                            }
                            PACKET_CG_PLAYER_SELECT_REQ sendData = new PACKET_CG_PLAYER_SELECT_REQ();
                            sendData.PlayerDBKey = player.PlayerDBKey;
                            GameUser.GetSession().SendPacket(sendData.Serialize());
                        });
                        InputManager.Instance.AddInputMap(0, ConsoleKey.UpArrow, () =>
                        {
                            PlayerManager.Instance.Seek(-1);
                        });
                        InputManager.Instance.AddInputMap(0, ConsoleKey.DownArrow, () =>
                        {
                            PlayerManager.Instance.Seek(1);
                        });
                    }
                    break;
                case GameMainPage.Item:
                    {
                        InputManager.Instance.Clear();
                        InputManager.Instance.AddInputMap(0, ConsoleKey.D1, () =>
                        {
                            PACKET_CG_ITEM_INFO_REQ sendData = new PACKET_CG_ITEM_INFO_REQ();
                            GameUser.GetSession().SendPacket(sendData.Serialize());
                        });
                        InputManager.Instance.AddInputMap(0, ConsoleKey.D2, () =>
                        {
                            PACKET_CG_SHOP_INFO_REQ sendData = new PACKET_CG_SHOP_INFO_REQ();
                            GameUser.GetSession().SendPacket(sendData.Serialize());
                        });
                        InputManager.Instance.AddInputMap(0, ConsoleKey.D3, () =>
                        {
                            var shopProduct = PlayerManager.Instance.GetSelectPlayer().ShopManager.GetSeekProduct();
                            var shop = PlayerManager.Instance.GetSelectPlayer().ShopManager.GetSeekShop();
                            if (shopProduct == null)
                            {
                                return;
                            }
                            PACKET_CG_SHOP_BUY_REQ sendData = new PACKET_CG_SHOP_BUY_REQ();
                            sendData.shopProductId = shopProduct.ShopProductId;
                            sendData.shopId = shop.ShopId;
                            GameUser.GetSession().SendPacket(sendData.Serialize());
                        });
                        InputManager.Instance.AddInputMap(0, ConsoleKey.UpArrow, () =>
                        {
                            PlayerManager.Instance.GetSelectPlayer().ShopManager.Seek(-1);
                        });
                        InputManager.Instance.AddInputMap(0, ConsoleKey.DownArrow, () =>
                        {
                            PlayerManager.Instance.GetSelectPlayer().ShopManager.Seek(1);
                        });
                    }
                    break;
            }
        }

        private string _mainLogo = string.Empty;
        public string MainLogo { get => _mainLogo; set => _mainLogo = value; }

        protected sealed override void PreInit()
        {

        }
        protected sealed override void OnInit()
        {

        }
        protected sealed override void OnRelease()
        {

        }
        protected sealed override void Update()
        {
            GameUI();
        }
        protected sealed override void OnEnter()
        {
            EventManager.Instance.AddEvent("Connectting", this);
            EventManager.Instance.AddEvent("ConnectionError", this);
            EventManager.Instance.AddEvent("Connection", this);
            EventManager.Instance.AddEvent("Disconnect", this);
            EventManager.Instance.AddEvent("AuthComplete", this);
            EventManager.Instance.AddEvent("PlayerList", this);
            EventManager.Instance.AddEvent("CreatePlayer", this);
            EventManager.Instance.AddEvent("SelectPlayer", this);
            EventManager.Instance.AddEvent("ItemInfo", this);
            EventManager.Instance.AddEvent("ShopInfo", this);
            EventManager.Instance.AddEvent("ShopBuy", this);
            MainPage = GameMainPage.Entry;
        }
        protected sealed override void OnExit()
        {
            EventManager.Instance.RemoveEvent("Connectting", this);
            EventManager.Instance.RemoveEvent("ConnectionError", this);
            EventManager.Instance.RemoveEvent("Connection", this);
            EventManager.Instance.RemoveEvent("Disconnect", this);
            EventManager.Instance.RemoveEvent("AuthComplete", this);
            EventManager.Instance.RemoveEvent("PlayerList", this);
            EventManager.Instance.RemoveEvent("CreatePlayer", this);
            EventManager.Instance.RemoveEvent("SelectPlayer", this);
            EventManager.Instance.RemoveEvent("ItemInfo", this);
            EventManager.Instance.RemoveEvent("ShopInfo", this);
            EventManager.Instance.RemoveEvent("ShopBuy", this);
            MainLogo = string.Empty;
            ConsoleManager.Instance.ConsoleClear();
            InputManager.Instance.Clear();
        }
        protected sealed override bool ReciveMessage(Message message)
        {
            ConsoleManager.Instance.ConsoleClear();
            switch (message.EventType)
            {
                case "Connectting":
                    MainPage = GameMainPage.Connecting;
                    break;
                case "ConnectionError":
                    MainPage = GameMainPage.ConnectError;
                    break;
                case "Connection":
                    MainPage = GameMainPage.Connecting;
                    GameUser = (GameUserObject)message.ExtraInfo;
                    GameUser.GetAccountImpl<GameBaseAccountClientImpl>()._SiteUserId = NetworkManager.Instance.LoginAuthInfo.SiteUserId;
                    GameUser.GetAccountImpl<GameBaseAccountClientImpl>()._Passport = NetworkManager.Instance.LoginAuthInfo.Passport;
                    GameUser.GetAccountImpl<GameBaseAccountClientImpl>()._IP = NetworkManager.Instance.LoginAuthInfo.IP;
                    GameUser.GetAccountImpl<GameBaseAccountClientImpl>()._Port = NetworkManager.Instance.LoginAuthInfo.Port;
                    GameUser.GetAccountImpl<GameBaseAccountClientImpl>()._PlatformType = NetworkManager.Instance.LoginAuthInfo.PlatformType;
                    break;
                case "Disconnect":
                    MainPage = GameMainPage.Disconnect;
                    break;
                case "AuthComplete":
                    MainPage = GameMainPage.AuthComplete;
                    break;
                case "PlayerList":
                    {
                        var playerInfos = (List<PlayerInfo>)message.ExtraInfo;
                        PlayerManager.Instance.Clear();
                        foreach (var playerInfo in playerInfos)
                        {
                            PlayerManager.Instance.CreatePlayer(playerInfo.PlayerDBKey
                                , playerInfo.SiteUserId
                                , playerInfo.PlayerName
                                , playerInfo.Version
                                , playerInfo.MatchToken);
                        }
                    }
                    break;
                case "CreatePlayer":
                    {
                        var playerInfo = (PlayerInfo)message.ExtraInfo;
                        PlayerManager.Instance.CreatePlayer(playerInfo.PlayerDBKey
                                , playerInfo.SiteUserId
                                , playerInfo.PlayerName
                                , playerInfo.Version
                                , playerInfo.MatchToken);
                    }
                    break;
                case "SelectPlayer":
                    {
                        var playerInfo = (PlayerInfo)message.ExtraInfo;
                        PlayerManager.Instance.SelectPlayer(playerInfo.PlayerDBKey);
                        MainPage = GameMainPage.Item;
                    }
                    break;
                case "ItemInfo":
                    {
                        var itemInfos = (List<ItemBaseInfo>)message.ExtraInfo;
                        PlayerManager.Instance.GetSelectPlayer().ItemManager.Clear();
                        foreach (var itemInfo in itemInfos)
                        {
                            PlayerManager.Instance.GetSelectPlayer().ItemManager.CreateItem(itemInfo.ParentItemId
                                , itemInfo.GroupIndex
                                , itemInfo.ItemType
                                , itemInfo.ItemId
                                , itemInfo.ItemLevel
                                , itemInfo.TotalValue
                                , itemInfo.RemainTime);
                        }
                    }
                    break;
                case "ShopInfo":
                    {
                        var shopInfos = (List<ShopInfo>)message.ExtraInfo;
                        PlayerManager.Instance.GetSelectPlayer().ShopManager.Clear();
                        foreach (var shopInfo in shopInfos)
                        {
                            PlayerManager.Instance.GetSelectPlayer().ShopManager.CreateShop(shopInfo);
                        }
                    }
                    break;
                case "ShopBuy":
                    {
                        ConsoleManager.Instance.ConsoleClear();
                        PACKET_CG_SHOP_BUY_RES response = (PACKET_CG_SHOP_BUY_RES)message.ExtraInfo;
                        PlayerManager.Instance.GetSelectPlayer().ShopManager.UpdateShop(response.shopId, response.shopProductInfo.shopProductId, response.shopProductInfo.buyCount);
                        if (response.changeProductInfo.shopProductId != 0)
                        {
                            PlayerManager.Instance.GetSelectPlayer().ShopManager.UpdateShop(response.shopId, response.changeProductInfo.shopProductId, response.changeProductInfo.buyCount);
                        }
                        foreach(var deleteItem in response.deleteItemInfo)
                        {
                            PlayerManager.Instance.GetSelectPlayer().ItemManager.UpdateItem(deleteItem.ItemId, deleteItem.Value, deleteItem.TotalValue);
                        }
                        foreach(var rewardItem in response.listRewardInfo)
                        {
                            PlayerManager.Instance.GetSelectPlayer().ItemManager.UpdateItem(rewardItem.ItemId, rewardItem.Value, rewardItem.TotalValue);
                        }
                        //퀘스트 업데이트
                        //response.listQuestData;
                    }
                    break;
            }
            return true;
        }

        private void GameUI()
        {
            //ConsoleManager.Instance.ConsoleClear();
            MainLogo = string.Empty;
            MainLogo += "======================================================================\n";
            MainLogo += "======================          게임 서버        =====================\n";
            MainLogo += "======================================================================\n";
            MainLogo += "                                                                      \n";

            switch (_mainPage)
            {
                case GameMainPage.Entry:
                    MainLogo += "1. 게임 서버 접속하기\n";
                    break;
                case GameMainPage.Connecting:
                    MainLogo += "접 속 중 ...\n";
                    break;
                case GameMainPage.ConnectError:
                    MainLogo += "접 속 실 패\n";
                    break;
                case GameMainPage.Disconnect:
                    MainLogo += "접 속 종 료\n";
                    break;
                case GameMainPage.AuthComplete:
                    MainLogo += "인증 완료\n";
                    MainLogo += "1. 로비 이동\n";
                    break;
                case GameMainPage.Player:
                    MainLogo += "1. 플레이어 리스트 2. 플레이어 생성  3. 플레이어 선택\n";
                    break;
                case GameMainPage.Item:
                    MainLogo += "1. 아이템 정보 2. 상점 정보 3. 아이템 구매\n";
                    break;
            }
            MainLogo += GameSubUI();
            ConsoleManager.Instance.SetBuffer(MainLogo);
        }

        private string GameSubUI()
        {
            string _subUI = string.Empty;
            switch (_mainPage)
            {
                case GameMainPage.Player:
                    {
                        _subUI += "플레이어 리스트\n";
                        if (PlayerManager.Instance.PlayerByDBKey.Count == 0)
                        {
                            _subUI += " no player data\n";
                            break;
                        }

                        _subUI += "PlayerDBKey\t\tPlayerName\t\tIsCur\n";
                        foreach(var player in PlayerManager.Instance.PlayerByDBKey)
                        {
                            _subUI += $"{player.Value.PlayerDBKey}\t{player.Value.PlayerName}";
                            var curPlayer = PlayerManager.Instance.GetSeekPlayer();
                            if (player.Value == curPlayer)
                            {
                                _subUI += $"\t<<";
                            }
                            else
                            {
                                _subUI += "\t    ";
                            }
                            _subUI += "\n";
                        }
                    }
                    break;
                case GameMainPage.Item:
                {
                        _subUI += "아이템 리스트\n";
                        if (PlayerManager.Instance.GetSelectPlayer().ItemManager.ItemById.Count == 0)
                        {
                            _subUI += "no item data\n";
                            break;
                        }
                        _subUI += "ItemId\t\tName\t\tTotalValue\n";
                        foreach(var item in PlayerManager.Instance.GetSelectPlayer().ItemManager.ItemById)
                        {
                            var itemData = DataTable<int, ItemListTable>.Instance.GetData(item.Value.ItemId);
                            _subUI += $"{item.Value.ItemId}\t\t{itemData.name}\t\t{item.Value.TotalValue}\n";
                        }
                    }
                    _subUI += "--------------------------------------------------------------------------------------\n";
                    {
                        _subUI += "샵 정보 \n";
                        if (PlayerManager.Instance.GetSelectPlayer().ShopManager.ShopByShopId.Count == 0)
                        {
                            _subUI += "no shop data\n";
                            break;
                        }
                        _subUI += $"ProductId\tName\t\tShopId\tBuyType\tBuyPrice\tItemId\tItemName\tValue\tIsCur\n";
                        ShopProduct shopProduct = PlayerManager.Instance.GetSelectPlayer().ShopManager.GetSeekProduct();
                        foreach(var shop in PlayerManager.Instance.GetSelectPlayer().ShopManager.ShopByShopId)
                        {
                            foreach (var product in shop.Value.Products)
                            {
                                var productTable = DataTable<int, ShopProductListTable>.Instance.GetData(product.ShopProductId);
                                var itemData = DataTable<int, ItemListTable>.Instance.GetData(productTable.itemId);
                                _subUI += $"{productTable.id}\t\t{productTable.name}\t\t{productTable.shopId}\t{(ShopBuyType)productTable.buyType}\t{productTable.buyPrice}\t{itemData.id}\t{itemData.name}\t{productTable.value}";
                                if (shopProduct == product)
                                {
                                    _subUI += $"\t<<";
                                }
                                else
                                {
                                    _subUI += $"\t  ";
                                }
                                _subUI += "\n";
                            }
                        }
                    }
                    break;
            }


            return _subUI;
        }
    }
}

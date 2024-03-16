#define SERVER
using System;
using System.Collections.Generic;
using Service.Core;
using Service.Net;
using GameBase.Template.GameBase;
using GameBase.Template.GameBase.Common;
using GameBase.Template.Shop.GameBaseShop.Common;
using GameBase.Template.GameBase.Table;
using System.Reflection.Metadata.Ecma335;

namespace GameBase.Template.Shop.GameBaseShop
{
	public partial class GameBaseShopTemplate : ShopTemplate
	{
		ImplObject _obj = null;
		static GameBaseShopImpl _Impl = null;

		public override GameBaseUserDB CreateUserDB()
		{
			return base.CreateUserDB();
		}

		public override void Init(TemplateConfig config, ServerType type)
		{
			base.Init(config, type);
			_Impl = new GameBaseShopImpl(type);
			//OnLoadData(config)
			// TODO : 서버 기동시 실행할 템플릿 관련 로직을 아래에 작성
		}

		public override void OnLoadData(TemplateConfig config)
		{
			// TODO : 로드할 데이터를 연결
			DataTable<int, ShopInfoTable>.Instance.Init(config.localPath + "/ShopInfo.csv");
			DataTable<int, ShopProductListTable>.Instance.Init(config.localPath + "/ShopProductList.csv");
		}
		public override void OnClientCreate(ImplObject userObject)
		{
			_obj = userObject;
			switch ((ObjectType)userObject.ObjectID)
			{
				case ObjectType.Master:
					{
						_obj.ShopImpl = new GameBaseShopMasterImpl(_obj);
					}
					break;
				case ObjectType.User:
					{
						_obj.ShopImpl = new GameBaseShopUserImpl(_obj);
					}
					break;
				case ObjectType.Login:
					{
						_obj.ShopImpl = new GameBaseShopLoginImpl(_obj);
					}
					break;
				case ObjectType.Game:
					{
						_obj.ShopImpl = new GameBaseShopGameImpl(_obj);
					}
					break;
				case ObjectType.Client:
					{
						_obj.ShopImpl = new GameBaseShopClientImpl(_obj);
					}
					break;
			}
			// TODO : 유저의 최초 생성시 필요한 DB관련 로직을 작성
		}

		public override void OnSetNewbie(ImplObject userObject)
		{
		}

		public override bool OnPlayerSelectPrepare(ImplObject userObject)
		{
			Reset(userObject);
            foreach(var shopInfo in DataTable<int, ShopInfoTable>.Instance.Values)
			{
				// db에 있는지 체크
				var DBShopProductList = userObject.GetUserDB().GetReadUserDB<GameBaseShopUserDB>(ETemplateType.Shop)._dbSlotContainer_DBShopTable.FindAll(slot => slot._DBData.shop_index == shopInfo.id);
                var productListTable = DataTable<int, ShopProductListTable>.Instance.Values.FindAll(productList => productList.shopId == shopInfo.id);

				foreach(var product in productListTable)
				{
                    var dbShopProductTable = DBShopProductList.Find(dbProduct => dbProduct._DBData.shop_product_index == product.id);
					if (dbShopProductTable == null)
					{
                        dbShopProductTable = userObject.GetUserDB().GetWriteUserDB<GameBaseShopUserDB>(ETemplateType.Shop)._dbSlotContainer_DBShopTable.Insert(userObject.GetUserDB().GetWriteUserDB<GameBaseShopUserDB>(ETemplateType.Shop)._dbSlotContainer_DBShopTable.GetVacantSlot());

                        dbShopProductTable._DBData.shop_index = shopInfo.id;
                        dbShopProductTable._DBData.shop_product_index = product.id;
                        dbShopProductTable._DBData.buy_count = 0;
                    }
					else
					{
						//현재 갱신은 없음
					}
                }
            }

            return true;
		}

		public T GetGameBaseShopImpl<T>() where T : ShopImpl
		{
			return _obj.ShopImpl as T;
		}

		public static GameBaseShopImpl GetGameBaseShopImpl()
		{
			return _Impl;
		}

		public override void OnTemplateUpdate(float dt)
		{
			// TODO : 템플릿 업데이트 사항 작성(유저X)
		}

		public override void OnClientUpdate(float dt)
		{
			// TODO : 템플릿 업데이트 사항 작성
		}

		public override void OnClientDelete(ImplObject userObject)
		{
			// TODO : 계정 초기화시 사용 템플릿에 보유한 내역듣 삭제
		}

		public override (List<ItemBaseInfo> listItemInfo, List<QuestCompleteParam> listQuestCompleteParam) OnAddItem(ImplObject userObject, int itemId, long value, int parentItemId, int groupIndex)
		{
			return (null, null);
		}

		public override (List<ItemBaseInfo> listItemInfo, List<QuestCompleteParam> listQuestCompleteParam) OnDeleteItem(ImplObject userObject, int itemId, long value, int parentItemId, int groupIndex)
		{
			return (null, null);
		}

		public override (List<ItemBaseInfo> listItemInfo, List<QuestCompleteParam> listQuestCompleteParam) AddRandomReward(ImplObject userObject, int classId, int grade, int kind, long value, int parentItemId, int groupIndex)
		{
			return (null, null);
		}

		public override bool OnHasItemId(ImplObject userObject, int itemId)
		{
			return false;
		}

		public override bool OnHasItemType(ImplObject userObject, int itemType)
		{
			return false;
		}

		public override bool OnHasItemSubType(ImplObject userObject, int subType)
		{
			return false;
		}

		private void Reset(ImplObject userObject) 
		{
            GameBaseShopUserImpl Impl = userObject.GetShopImpl<GameBaseShopUserImpl>();
			Impl.ShopInfoList.Clear();
		}

        private (GServerCode, List<ItemBaseInfo>, List<QuestCompleteParam>) UpdateBuyItem(ImplObject userObject, ShopProductListTable shopProductListTable)
		{
			List<ItemBaseInfo> listUpdateItem = new List<ItemBaseInfo>();
			List<QuestCompleteParam> listQUestCompleteParam = new List<QuestCompleteParam>();
            ShopBuyType buyType = (ShopBuyType)shopProductListTable.buyType;
			GServerCode error = GServerCode.SUCCESS;

            switch (buyType)
			{
				case ShopBuyType.Cash:
					error = GServerCode.Error;
					break;
				case ShopBuyType.Gold:
				case ShopBuyType.Diamond:
					var res = ConvertToItemId(buyType);
					if (res.Item1 != GServerCode.SUCCESS)
					{
						error = res.Item1;
						break;
					}
					var res2 = GameBaseTemplateContext.DeleteItem(userObject.GetSession().GetUid(), res.Item2, (int)shopProductListTable.buyPrice);
					listUpdateItem = res2.listItemInfo;
					listQUestCompleteParam = res2.listQuestCompleteParam;
					break;
				case ShopBuyType.Free:
					break;
			}

			return (error, listUpdateItem, listQUestCompleteParam);
        }

        private void UpdateProduct(ImplObject userObject, ShopProductListTable shopProductListTable)
		{
			var dbShopProductList = userObject.GetUserDB().GetWriteUserDB<GameBaseShopUserDB>(ETemplateType.Shop)._dbSlotContainer_DBShopTable;
			var dbProduct = dbShopProductList.Find(product => product._DBData.shop_product_index == shopProductListTable.id);
            dbProduct.

        }

        private void CreateRewardItem(ImplObject userObject)
		{

		}

		private (GServerCode, int) ConvertToItemId(ShopBuyType buyType)
		{
            switch (buyType)
            {
                case ShopBuyType.Gold:
                case ShopBuyType.Diamond:
                    var Item = DataTable<int, ItemListTable>.Instance.GetData(table => table.itemSubType == (int)ItemSubType.Gold);
                    if (Item == null)
					{
						return (GServerCode.Error, -1);
					}
					return (GServerCode.SUCCESS, Item.id);
                case ShopBuyType.Cash:
				case ShopBuyType.Free:
					return (GServerCode.Error, -1);
				default:
					return (GServerCode.Error, -1);
            }
        }
    }
}

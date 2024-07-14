#define SERVER
using System;
using System.Collections.Generic;
using Service.Core;
using Service.Net;
using GameBase.Template.GameBase;
using GameBase.Template.GameBase.Common;
using GameBase.Template.Item.GameBaseItem.Common;
using GameBase.Template.GameBase.Table;

namespace GameBase.Template.Item.GameBaseItem
{
	public partial class GameBaseItemTemplate : ItemTemplate
	{
		ImplObject _obj = null;
		static GameBaseItemImpl _Impl = null;

        public override GameBaseUserDB CreateUserDB()
        {
			return new GameBaseItemUserDB();
        }

		public override void Init(TemplateConfig config, ServerType type)
		{
			base.Init(config, type);
			_Impl = new GameBaseItemImpl(type);
			//OnLoadData(config)
			// TODO : 서버 기동시 실행할 템플릿 관련 로직을 아래에 작성
		}

		public override void OnLoadData(TemplateConfig config)
		{
            // TODO : 로드할 데이터를 연결
        }

		public override void OnClientCreate(ImplObject userObject)
		{
			_obj = userObject;
			switch ((ObjectType)userObject.ObjectID)
			{
				case ObjectType.Master:
					{
						_obj.ItemImpl = new GameBaseItemMasterImpl(_obj);
					}
					break;
				case ObjectType.User:
					{
						_obj.ItemImpl = new GameBaseItemUserImpl(_obj);
					}
					break;
				case ObjectType.Login:
					{
						_obj.ItemImpl = new GameBaseItemLoginImpl(_obj);
					}
					break;
				case ObjectType.Game:
					{
						_obj.ItemImpl = new GameBaseItemGameImpl(_obj);
					}
					break;
				case ObjectType.Client:
					{
						_obj.ItemImpl = new GameBaseItemGameImpl(_obj);
					}
					break;
			}
			// TODO : 유저의 최초 생성시 필요한 DB관련 로직을 작성
		}

        public override void OnSetNewbie(ImplObject userObject)
        {
			var defaultItem = DataTable<string, CommonDataTable>.Instance.ValuePairs["default_item"].value2;
			if (defaultItem != null)
			{
				for (int i = 0; i < defaultItem.Count; i+= 2)
				{
					int itemId = defaultItem[i];
					int itemCount = defaultItem[i + 1];
					var itemTable = DataTable<int, ItemListTable>.Instance.GetData(itemId);
					if (itemTable == null)
					{
						throw new Exception("OnSetNewbie Not Found Item");
					}

					var writeItemDB = userObject.UserDB.GetWriteUserDB<GameBaseItemUserDB>(ETemplateType.Item)._dbSlotContainer_DBItemTable;
					short writeSlot = writeItemDB.GetVacantSlot();
					var newItem = writeItemDB.Insert(writeSlot);
					newItem._DBData.item_type = (int)itemTable.itemType;
                    newItem._DBData.item_id = itemId;
					newItem._DBData.item_count = itemCount;
                }
			}
        }

        public override bool OnPlayerSelectPrepare(ImplObject userObject)
        {
            GameBaseItemClientImpl Impl = userObject.GetItemImpl<GameBaseItemClientImpl>();
			// 상품 초기화 등등 작업
			return true;
        }

        public T GetGameBaseItemImpl<T>() where T : ItemImpl
		{
			return _obj.ItemImpl as T;
		}

		public static GameBaseItemImpl GetGameBaseItemImpl()
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
			ItemListTable itemList = DataTable<int, ItemListTable>.Instance.GetData(itemId);
			if (itemList == null)
			{
				throw new Exception("No Exist Table");
			}

			List<ItemBaseInfo> listUpdateItem = new List<ItemBaseInfo>();
			List<QuestCompleteParam> listQUestCompleteParam = new List<QuestCompleteParam>();
			switch((ItemType)itemList.itemType)
			{
				case ItemType.Resource:
					{
						listUpdateItem = UpdateItemResource(userObject, (ItemSubType)itemList.itemSubType, itemId, value, itemList.maxValue);
					}
					break;
			}

			if (listUpdateItem != null)
			{
				listQUestCompleteParam.Add(new QuestCompleteParam { QuestType = (int)QuestType.UpdateItem, EndId = itemId, Value = value });
			}
			return (listUpdateItem, listQUestCompleteParam);
		}

		public override (List<ItemBaseInfo> listItemInfo, List<QuestCompleteParam> listQuestCompleteParam) OnDeleteItem(ImplObject userObject, int itemId, long value, int parentItemId, int groupIndex)
		{
            ItemListTable itemList = DataTable<int, ItemListTable>.Instance.GetData(itemId);
            if (itemList == null)
            {
                throw new Exception("No Exist Table");
            }

            List<ItemBaseInfo> listUpdateItem = new List<ItemBaseInfo>();
            List<QuestCompleteParam> listQUestCompleteParam = new List<QuestCompleteParam>();
            switch ((ItemType)itemList.itemType)
            {
                case ItemType.Resource:
                    {
                        listUpdateItem = UpdateItemResource(userObject, (ItemSubType)itemList.itemSubType, itemId, value * -1, itemList.maxValue);
                    }
                    break;
            }

            if (listUpdateItem != null)
            {
                listQUestCompleteParam.Add(new QuestCompleteParam { QuestType = (int)QuestType.UpdateItem, EndId = itemId, Value = value });
            }
            return (listUpdateItem, listQUestCompleteParam);
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

		private List<ItemBaseInfo> UpdateItemResource(ImplObject userObject, ItemSubType itemSubType, int itemId, long value, long maxValue)
		{
			//var dbItemTableList = userObject.GetUserDB().GetWriteUserDB<GameBaseItemUserDB>(ETemplateType.Item)._dbSlotContainer_DBItemTable;
			List<ItemBaseInfo> itemInfo = null;
			if (value < 0
				&& (itemSubType == ItemSubType.Gold || itemSubType == ItemSubType.Diamond || itemSubType == ItemSubType.Stamina))
			{
				itemInfo = DeleteResource(userObject, itemSubType, itemId, value);
            }
			else
			{
				itemInfo = UpdateItemCount(userObject, ItemType.Resource, itemId, value, maxValue);
			}

			return itemInfo;
		}

		List<ItemBaseInfo> UpdateItemCount(ImplObject userObject, ItemType itemType, int itemId, long value, long maxValue)
		{
			var readItemDB = userObject.UserDB.GetReadUserDB<GameBaseItemUserDB>(ETemplateType.Item)._dbSlotContainer_DBItemTable;
			var readItem = readItemDB.Find(slot => slot._DBData.item_id == itemId);
			List<ItemBaseInfo> listItemBaseInfo = new List<ItemBaseInfo>();
			if (readItem == null)
			{
				if (value < 0)
				{
					throw new Exception("Not Found Item");
				}
				var writeItemDBTable = userObject.UserDB.GetWriteUserDB<GameBaseItemUserDB>(ETemplateType.Item)._dbSlotContainer_DBItemTable;
				short vacantSlot = writeItemDBTable.GetVacantSlot();
				var writeItemDB = writeItemDBTable.Insert(vacantSlot);
				writeItemDB._DBData.item_id = itemId;
				writeItemDB._DBData.item_type = (int)itemType;
				writeItemDB._DBData.item_count = value;
				ItemBaseInfo itemInfo = new ItemBaseInfo();
				itemInfo.ItemId = itemId;
				itemInfo.ItemType = (int)itemType;
				itemInfo.Value = value;
				itemInfo.TotalValue = value;
				listItemBaseInfo.Add(itemInfo);
            }
			else
			{
				long count = readItem._DBData.item_count + value;
				if (count < 0)
				{
					throw new Exception("Wrong Item Count");
				}
				short writeSlot = readItem._nSlot;
				var writeItemDB = userObject.UserDB.GetWriteUserDB<GameBaseItemUserDB>(ETemplateType.Item)._dbSlotContainer_DBItemTable.GetWriteData(writeSlot);
				writeItemDB._DBData.item_count = (int)Math.Min(count, maxValue);
				ItemBaseInfo itemInfo = new ItemBaseInfo();
				itemInfo.ItemId = itemId;
				itemInfo.ItemType = (int)itemType;
				itemInfo.Value = value;
				itemInfo.TotalValue = writeItemDB._DBData.item_count;
                listItemBaseInfo.Add(itemInfo);
            }

			return listItemBaseInfo;
		}


        private List<ItemBaseInfo> DeleteResource(ImplObject userObject, ItemSubType itemSubType, int itemId, long value) 
		{
			List<ItemBaseInfo> listItemInfo = new List<ItemBaseInfo>();
			var dbItem = userObject.UserDB.GetReadUserDB<GameBaseItemUserDB>(ETemplateType.Item)._dbSlotContainer_DBItemTable.Find(slot => slot._DBData.item_id == itemId);
			if (dbItem == null)
			{
				throw new Exception("DeleteResource Not FInd DB Table");
			}

			// 유료 캐시부터 제거
			int cashResourceSubType = 0;
			switch (itemSubType)
			{
				case ItemSubType.Gold:
					{
						cashResourceSubType = (int)ItemSubType.CashGold;
						break;
					}
				case ItemSubType.Diamond:
					{
						cashResourceSubType = (int)ItemSubType.CashDiamond;
						break;
					}
				case ItemSubType.Stamina:
					{
						cashResourceSubType = (int)ItemSubType.CashStamina;
						break;
					}
				default:
					{
						throw new Exception("DeleteResouce Wrong Resource Type");
					}
			}

			long remainValue = value;
			var itemTable = DataTable<int, ItemListTable>.Instance.GetData(table => table.itemSubType == cashResourceSubType);
            if (itemTable != null)
			{
				var readData = userObject.UserDB.GetReadUserDB<GameBaseItemUserDB>(ETemplateType.Item)._dbSlotContainer_DBItemTable;
				int nSlot = 0;
				readData.BreakableForEach(slot =>
				{
					if (slot._DBData.item_id == itemTable.id)
					{
						nSlot = slot._nSlot;
						return true;
					}
					return false;
				});
				var dbCashResource = userObject.UserDB.GetWriteUserDB<GameBaseItemUserDB>(ETemplateType.Item)._dbSlotContainer_DBItemTable;
				var cashItemDB = dbCashResource.GetWriteData((short)nSlot);
                if (cashItemDB != null)
				{
                    if (dbItem._DBData.item_count + cashItemDB._DBData.item_count < remainValue)
					{
						throw new Exception("Not Enough Resource");
					}

                    if (cashItemDB._DBData.item_count > 0)
					{
						if (cashItemDB._DBData.item_count >= remainValue)
						{
							cashItemDB._DBData.item_count -= remainValue;
							remainValue = 0;
                        }
						else 
						{
							remainValue -= cashItemDB._DBData.item_count;
							cashItemDB._DBData.item_count = 0;
                        }

						var itemInfo = new ItemBaseInfo();
						itemInfo.ItemType = itemTable.itemType;
						itemInfo.ItemId = itemTable.id;
						itemInfo.Value = (value - remainValue) * -1;
						itemInfo.TotalValue = cashItemDB._DBData.item_count;
						listItemInfo.Add(itemInfo);
                    }
				}
            }

			if (remainValue > 0)
			{
				var dbFreeResource = userObject.UserDB.GetWriteUserDB<GameBaseItemUserDB>(ETemplateType.Item)._dbSlotContainer_DBItemTable.GetWriteData(dbItem._nSlot);
				if (dbFreeResource._DBData.item_count < remainValue)
				{
					throw new Exception("Not Enough Resource");
				}
                dbFreeResource._DBData.item_count = dbFreeResource._DBData.item_count - remainValue;
				var itemInfo = new ItemBaseInfo();
				itemInfo.ItemType = dbFreeResource._DBData.item_type;
				itemInfo.ItemId = itemId;
				itemInfo.Value = remainValue * -1;
				itemInfo.TotalValue = dbFreeResource._DBData.item_count;
				listItemInfo.Add(itemInfo);
            }

			return listItemInfo;
        }
	}
}

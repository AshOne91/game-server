#define SERVER
using System;
using System.Collections.Generic;
using Service.Net;
using Service.Core;
using GameBase.Template.GameBase;

namespace GameBase.Template.Shop.GameBaseShop.Common
{
	public partial class GameBaseShopProtocol
	{
		Dictionary<ushort, ControllerDelegate> MessageControllers = new Dictionary<ushort, ControllerDelegate>();

		public GameBaseShopProtocol()
		{
			Init();
		}

		void Init()
		{
			MessageControllers.Add(PACKET_CG_SHOP_INFO_REQ.ProtocolId, CG_SHOP_INFO_REQ_CONTROLLER);
			MessageControllers.Add(PACKET_CG_SHOP_INFO_RES.ProtocolId, CG_SHOP_INFO_RES_CONTROLLER);

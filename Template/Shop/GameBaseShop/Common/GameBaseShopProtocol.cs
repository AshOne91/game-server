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
		}

		public virtual bool OnPacket(ImplObject userObject, ushort protocolId, Packet packet)
		{
			ControllerDelegate controllerCallback;
			if(MessageControllers.TryGetValue(protocolId, out controllerCallback) == false)
			{
				return false;
			}
			controllerCallback(userObject, packet);
			return true;
		}

	}
}

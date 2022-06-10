#define SERVER
using System;
using System.Collections.Generic;
using Service.Net;
using Service.Core;

namespace GameBase.Template.GameBase.Common
{
	public partial class GameBaseProtocol
	{
		public static Dictionary<ushort, ControllerDelegate> MessageControllers = new Dictionary<ushort, ControllerDelegate>();

		public GameBaseProtocol()
		{
			Init();
		}

		void Init()
		{
		}

		public virtual bool OnPacket(UserObject userObject, ushort protocolId, Packet packet)
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

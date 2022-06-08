#define SERVER
using System;
using System.Collections.Generic;
using Service.Net;
using Service.Core;

namespace GameBase.Common
{
	public partial class GameBaseProtocol
	{
		public MessageController _messageController = null;

		public GameBaseProtocol(UserObject obj)
		{
			_messageController = new MessageController(obj);
			Init();
		}

		void Init()
		{
		}

		public virtual bool OnPacket(ushort protocolId, Packet packet)
		{
			return _messageController.OnRecevice(protocolId, packet);
		}


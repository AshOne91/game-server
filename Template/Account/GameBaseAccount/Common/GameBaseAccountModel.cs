#define SERVER
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Numerics;
using Service.Net;
using Service.Core;

namespace GameBase.Template.Account.GameBaseAccount.Common
{
	public class PlayerInfo : IPacketSerializable
	{
		/// <sumary>
		/// 플레이어 DB Key
		/// </sumary>
		public ulong PlayerDBKey = new ulong();
		/// <sumary>
		/// 플레이어 네임
		/// </sumary>
		public ulong PlayerName = new ulong();
		public void Serialize(Packet packet)
		{
			packet.Write(PlayerDBKey);
			packet.Write(PlayerName);
		}
		public void Deserialize(Packet packet)
		{
			packet.Read(ref PlayerDBKey);
			packet.Read(ref PlayerName);
		}
		public string GetLog()
		{
			string log = "";
			FieldInfo[] fields = this.GetType().GetFields();
			foreach (FieldInfo field in fields)
			{
				log += string.Format("{{0}}={{1}}\r\n", field.Name, field.GetValue(this).ToString());
			}
			return log;
		}
	}
}

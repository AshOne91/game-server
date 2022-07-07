#define SERVER
using System;
using System.Collections.Generic;
using Service.Core;
using Service.Net;
using GameBase.Template.GameBase;
using GameBase.Template.GameBase.Common;
using GameBase.Template.Item.GameBaseItem.Common;

namespace GameBase.Template.Item.GameBaseItem
{
	public partial class GameBaseItemImpl : ItemImpl
	{
		public GameBaseItemImpl(ServerType type) : base(type){}
		// TODO : 서버에서 사용 될 변수 선언 및 함수 구현
	}

	public partial class GameBaseItemMasterImpl : ItemImpl
	{
		public GameBaseItemMasterImpl(ImplObject obj) : base(obj){}
		// TODO : ImplObject에서 사용 될 변수 선언 및 함수 구현
	}

	public partial class GameBaseItemUserImpl : ItemImpl
	{
		public GameBaseItemUserImpl(ImplObject obj) : base(obj){}
		// TODO : ImplObject에서 사용 될 변수 선언 및 함수 구현
	}

	public partial class GameBaseItemLoginImpl : ItemImpl
	{
		public GameBaseItemLoginImpl(ImplObject obj) : base(obj){}
		// TODO : ImplObject에서 사용 될 변수 선언 및 함수 구현
	}

	public partial class GameBaseItemGameImpl : ItemImpl
	{
		public GameBaseItemGameImpl(ImplObject obj) : base(obj){}
		// TODO : ImplObject에서 사용 될 변수 선언 및 함수 구현
	}
}

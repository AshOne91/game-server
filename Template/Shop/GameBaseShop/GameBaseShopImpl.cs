#define SERVER
using System;
using System.Collections.Generic;
using Service.Core;
using Service.Net;
using GameBase.Template.GameBase;
using GameBase.Template.GameBase.Common;
using GameBase.Template.Shop.GameBaseShop.Common;

namespace GameBase.Template.Shop.GameBaseShop
{
	public partial class GameBaseShopImpl : ShopImpl
	{
		public GameBaseShopImpl(ServerType type) : base(type){}
		// TODO : 서버에서 사용 될 변수 선언 및 함수 구현
	}

	public partial class GameBaseShopMasterImpl : ShopImpl
	{
		public GameBaseShopMasterImpl(ImplObject obj) : base(obj){}
		// TODO : ImplObject에서 사용 될 변수 선언 및 함수 구현
	}

	public partial class GameBaseShopUserImpl : ShopImpl
	{
		public GameBaseShopUserImpl(ImplObject obj) : base(obj){}
		// TODO : ImplObject에서 사용 될 변수 선언 및 함수 구현
	}

	public partial class GameBaseShopLoginImpl : ShopImpl
	{
		public GameBaseShopLoginImpl(ImplObject obj) : base(obj){}
		// TODO : ImplObject에서 사용 될 변수 선언 및 함수 구현
	}

	public partial class GameBaseShopGameImpl : ShopImpl
	{
		public GameBaseShopGameImpl(ImplObject obj) : base(obj){}
		// TODO : ImplObject에서 사용 될 변수 선언 및 함수 구현
	}

	public partial class GameBaseShopClientImpl : ShopImpl
	{
		public GameBaseShopClientImpl(ImplObject obj) : base(obj){}
		// TODO : ImplObject에서 사용 될 변수 선언 및 함수 구현
	}
}

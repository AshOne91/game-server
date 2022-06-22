#define SERVER
using System;
using System.Collections.Generic;
using Service.Core;
using Service.Net;
using GameBase.Template.GameBase;
using GameBase.Template.GameBase.Common;
using GameBase.Template.GameBase.Common;

namespace GameBase.Template.GameBase
{
	public partial class GameBaseImpl : BaseImpl
	{
		public GameBaseImpl(ServerType type) : base(type){}
		// TODO : 서버에서 사용 될 변수 선언 및 함수 구현
	}

	public partial class GameBaseMasterImpl : BaseImpl
	{
		public GameBaseMasterImpl(ImplObject obj) : base(obj){}
		// TODO : ImplObject에서 사용 될 변수 선언 및 함수 구현
	}

	public partial class GameBaseUserImpl : BaseImpl
	{
		public GameBaseUserImpl(ImplObject obj) : base(obj){}
		// TODO : ImplObject에서 사용 될 변수 선언 및 함수 구현
	}

	public partial class GameBaseLoginImpl : BaseImpl
	{
		public GameBaseLoginImpl(ImplObject obj) : base(obj){}
		// TODO : ImplObject에서 사용 될 변수 선언 및 함수 구현
	}

	public partial class GameBaseGameImpl : BaseImpl
	{
		public GameBaseGameImpl(ImplObject obj) : base(obj){}
		// TODO : ImplObject에서 사용 될 변수 선언 및 함수 구현
	}
}

#define SERVER
using System;

namespace GameBase.Template.Shop.GameBaseShop
{
	// TODO : 템플릿에서 사용할 열거형을 정의합니다.

	public enum ShopStatusType
	{
		None = 0
	}

	public enum ShopBuyType
	{
		None = 0,
		Gold = 1,
		Diamond = 2,
		Cash = 3,
		Free = 4
	}
}

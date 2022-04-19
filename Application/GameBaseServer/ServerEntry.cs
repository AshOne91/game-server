using System;
using Service.Core;

namespace GameBaseServer
{
    class ServerEntry
    {
        static void Main(string[] args)
        {
            //공용 설정파일 추가 예정
            Console.Title = "test";

            Logger.Default = new Logger();
            Logger.Default.Create(true, "MustConfigJsonRead");
            Logger.Default.Log(ELogLevel.Always, "ConfigFileReadPlease...");
        }
    }
}

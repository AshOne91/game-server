using System;
using System.IO;
using Service.Core;

namespace GameBaseServer
{
    class ServerEntry
    {
        static void Main(string[] args)
        {
            try
            {
                //공용 설정파일 추가 예정
                Console.Title = "test";

                Logger.Default = new Logger();
                Logger.Default.Create(true, "MustConfigJsonRead");
                Logger.Default.Log(ELogLevel.Always, "ConfigFileReadPlease...");

#if (!DEBUG)
                using (StreamReader reader = new StreamReader("gamebaseserver-config.json"));
#else
                using (StreamReader reader = new StreamReader(""))
#endif
                {


                }
            }
            catch (Exception e)
            {
                Logger.WriteExceptionLog(e);
            }
            finally
            {

            }
            return;
        }
    }
}

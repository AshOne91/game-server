using System;
using System.Collections.Generic;
using System.Text;
using Service.Core;
using Service.DB;
using Service.Net;

namespace GameBase.Template.GameBase
{
    public class GameBaseDBManager : DBManager
    {
        private HashSet<short/*DBIndex*/> _setGameDBIndex;
        public GameBaseDBManager(Logger logFunc) : base(logFunc)
        {

        }
        ~GameBaseDBManager()
        {

        }

        public void DBLoad_Request(int ServerID, Action complateCallback = null)
        {
            DBGlobal_DBList_LoadAll query = new DBGlobal_DBList_LoadAll();
            PushQueryGlobal(0, query, () => {
                bool IsSuccess = false;
                if (query.IsSuccess())
                {
                    foreach (var simpleInfo in query._dbSampleInfos)
                    {
                        _GetDBConfig().SetDBSimpleInfo(simpleInfo);
                        //FIXME FIXME
                        if (simpleInfo._dbType == EDBType.Game)
                        {
                            _setGameDBIndex.Add(simpleInfo._dbIndex);
                        }
                    }

                    if (SetDB(query._dbSampleInfos))
                    {
                        IsSuccess = true;
                    }
                }
                if (!IsSuccess)
                {
                    _logFunc.Log(ELogLevel.Fatal, "DBList Load Fail...");
                }

                if (complateCallback != null)
                {
                    complateCallback();
                }
            });
        }

        public void PushQueryGlobal(ulong playerDBKey, QueryBase query, Action completCallback = null)
        {

        }
        public void PushQueryGlobal(QueryBase query, Action completCallback = null)
        {

        }

        public void PushQueryGame(int threadSeed, QueryBase query, Action completeCallback = null)
        {

        }

        public void PushQueryGame(QueryBase query, Action completeCallback = null)
        {

        }

        public void PushQuerySharding(int threadSeed, QueryBase query, Action completeCallback = null)
        {

        }

        private void _PushQueryDB(EDBType type, short dbIndex, int threadSeed, QueryBase query, Action completeCallback, string fuctionName)
        {

        }
    }
}

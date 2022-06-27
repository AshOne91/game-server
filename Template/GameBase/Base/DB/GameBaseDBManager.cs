using System;
using System.Collections.Generic;
using System.Text;
using System.Reflection;
using Service.Core;
using Service.DB;
using Service.Net;

namespace GameBase.Template.GameBase
{
    public class GameBaseDBManager : DBManager
    {
        private HashSet<short/*DBIndex*/> _setGameDBIndex = new HashSet<short>();
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
                        simpleInfo._threadCount = 2;
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

        public void PushQueryGlobal(ulong playerDBKey, QueryBase query, Action completeCallback = null)
        {
            _PushQueryDB(EDBType.Global, 0, playerDBKey, query, completeCallback, MethodBase.GetCurrentMethod().Name);
        }
        public void PushQueryGlobal(QueryBase query, Action completeCallback = null)
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

        private void _PushQueryDB(EDBType type, short dbIndex, ulong threadSeed, QueryBase query, Action completeCallback, string fuctionName)
        {
            DBSimpleInfo simpleInfo = _GetDBConfig().GetDBSimpleInfo(type, dbIndex);
            if (simpleInfo == null)
            {
                _logFunc.Log(ELogLevel.Err, "DBSimpleInfo Not Exist! DBType=" + type.ToString());
                return;
            }

            if (threadSeed > 0)
            {
                //FixMe
                /*switch (DBType.Enum())
                {
                    case EDBType::Log:
                    case EDBType::Push:
                    case EDBType::Redis1:
                    case EDBType::Redis2:
                        {
                            CGUser* pUser = CGUserManager::Instance()->FindGUserByUserDBKey(static_cast<DBKEY64>(ThreadSeed));
                            if (pUser && pUser->IsStatisticsTest())
                            {
                                if (CompleteCallback)
                                {
                                    CompleteCallback();
                                }
                                SAFE_DELETE(pQuery);
                                return;
                            }
                        }
                        break;
                }*/
            }

            query.SetCompleteCallback(completeCallback);

            var threadIndex = _Mod(threadSeed, simpleInfo._threadCount);
            int DBKey = _MakeDBKey(type, dbIndex, (byte)threadIndex);
            try
            {
                _PushQuery(DBKey, query);
            }
            catch (Exception e)
            {
                _logFunc.Log(ELogLevel.Fatal, "[{0}] {1}", MethodBase.GetCurrentMethod().Name, e.Message);
            }

        }

        ulong _Mod(ulong dividend, ulong divisor)
        {
            return dividend % (divisor != 0 ? divisor : 1);
        }


    }
}

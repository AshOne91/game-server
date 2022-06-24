using System;
using System.Collections.Generic;
using System.Data.Odbc;
using System.Text;
using Service.DB;

namespace GameBase.Template.GameBase
{
    public class DBGlobal_DBList_LoadAll : QueryBase
    {
        //Out
        public List<DBSimpleInfo> _dbSampleInfos = new List<DBSimpleInfo>();
        public DBGlobal_DBList_LoadAll() { _dbSampleInfos.Clear(); }
        ~DBGlobal_DBList_LoadAll() { }

        public override void vRun(AdoDB adoDB)
        {
            try
            {
                QueryBuilder query = new QueryBuilder("gp_db_list_load");

                adoDB.Execute(query);

                while (adoDB.RecordWhileNotEOF())
                {
                    DBSimpleInfo simpleInfo = new DBSimpleInfo();

                    simpleInfo._dbIndex = adoDB.RecordGetValue("index");
                    simpleInfo._dbType = (EDBType)(ushort)adoDB.RecordGetValue("type");
                    simpleInfo._dbName = adoDB.RecordGetStrValue("name");
                    simpleInfo._dbID = adoDB.RecordGetStrValue("id");
                    simpleInfo._dbPW = adoDB.RecordGetStrValue("pw");
                    simpleInfo._dbIP = adoDB.RecordGetStrValue("ip");
                    simpleInfo._slaveDBIP = adoDB.RecordGetStrValue("slaveIp");
                    simpleInfo._dbPort = adoDB.RecordGetValue("port");
                }
                adoDB.RecordEnd();
            }
            catch(OdbcException e)
            {
                adoDB.RecordEnd();
                _strResult = "[gp_db_list_load] " + e.Message;
            }
        }

        public override string vGetName()
        {
            return "DBAccount_DBList_LoadAll";
        }
    }
}

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
                //https://social.msdn.microsoft.com/Forums/en-US/c00cb4e0-8162-495c-871f-34fe692dec6e/call-a-stored-procedure-with-parameter-in-c-and-mysql?forum=aspwebforms
                //https://stackoverflow.com/questions/3583074/execute-parameterized-sql-storedprocedure-via-odbc
                //mssql과 mysql odbc의 storedProcedure 호출 방법이 다름
                QueryBuilder query = new QueryBuilder("call gp_db_list_load()");

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
                    simpleInfo._slaveDBIP = adoDB.RecordGetStrValue("slave_ip");
                    simpleInfo._dbPort = adoDB.RecordGetValue("port");
                    _dbSampleInfos.Add(simpleInfo);
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

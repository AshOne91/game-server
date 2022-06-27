using System;
using System.Collections.Generic;
using System.Text;
using Service.DB;
using Service.Net;
using System.Data.Odbc;

namespace GameBase.Template.GameBase
{
    public class DBGlobal_PlatformAuth : QueryBaseValidate
    {
        //IN
        public string _site_user_id;
        public int _platform_type;

        //Out
        public ulong _user_indx;
        public int _sharding_index;
        public int _gamedb_index;
        public int _logdb_index;
        public bool _is_google_link;
        public bool _is_apple_link;
        public bool _is_facebook_link;
        public bool _is_kakao_link;

        public DBGlobal_PlatformAuth(UserObject obj):base(obj) {}

        public override void vRun(AdoDB adoDB)
        {
            try
            {
                QueryBuilder query = new QueryBuilder("call gp_server_platform_auth(?,?)");
                query.SetInputParam("@p_platform_type", _platform_type);
                query.SetInputParam("@p_site_user_id", _site_user_id);

                adoDB.Execute(query);

                if (adoDB.RecordNotEOF())
                {
                    _user_indx = adoDB.RecordGetValue("user_indx");
                    _sharding_index = adoDB.RecordGetValue("sharding_index");
                    _gamedb_index = adoDB.RecordGetValue("gamedb_index");
                    _logdb_index = adoDB.RecordGetValue("logdb_index");
                    _is_google_link = adoDB.RecordGetValue("is_google_link");
                    _is_apple_link = adoDB.RecordGetValue("is_apple_link");
                    _is_facebook_link = adoDB.RecordGetValue("is_facebook_link");
                }
                else
                {
                    _strResult = "[gp_server_platform_auth] No Result!";
                }
                adoDB.RecordEnd();
            }
            catch (OdbcException e)
            {
                adoDB.RecordEnd();
                _strResult = "[gp_server_platform_auth] " + e.Message;
            }
        }

        public override string vGetName()
        {
            return "DBGlobal_get_user_config";
        }
    }
}

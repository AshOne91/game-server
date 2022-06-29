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
        public ulong _account_db_key;
        public string _encode_account_id;
        public string _account_status;
        public DateTime _block_endtime;
        public bool _is_withdraw;
        public DateTime _withdraw_time;
        public int _withdraw_cancel_count;
        public bool _is_google_link;
        public bool _is_apple_link;
        public bool _is_facebook_link;

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
                    _account_db_key = adoDB.RecordGetValue("account_db_key");
                    _encode_account_id = adoDB.RecordGetStrValue("encode_account_id");
                    _account_status = adoDB.RecordGetStrValue("account_status");
                    _block_endtime = adoDB.RecordGetTimeValue("block_endtime");
                    _is_withdraw = adoDB.RecordGetValue("is_withdraw");
                    _withdraw_time = adoDB.RecordGetTimeValue("withdraw_time");
                    _withdraw_cancel_count = adoDB.RecordGetValue("withdraw_cancel_count");
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
            return "gp_server_platform_auth";
        }
    }
    public class DBGlobal_GetUser : QueryBaseValidate
    {
        // IN
        public ulong _account_db_key;
        public int _server_id;

        //Out
        public ulong _user_db_key;
        public short _gamedb_idx;
        public short _logdb_idx;
        public int _block_status;
        public DateTime _block_end_time;
        public DateTime _recent_login_time;
        public DateTime _recent_logout_time;
        public byte _gm_level;

        public DBGlobal_GetUser(UserObject obj) : base(obj) { }
        public override void vRun(AdoDB adoDB)
        {
            try
            {
                QueryBuilder query = new QueryBuilder("call gp_server_get_user(?,?)");
                query.SetInputParam("@p_account_db_key", _account_db_key);
                query.SetInputParam("@p_server_id", _server_id);

                adoDB.Execute(query);

                if (adoDB.RecordNotEOF())
                {
                    _user_db_key = adoDB.RecordGetValue("user_db_key");
                    _gamedb_idx = adoDB.RecordGetValue("gamedb_idx");
                    _logdb_idx = adoDB.RecordGetValue("logdb_idx");
                    _block_status = adoDB.RecordGetValue("block_status");
                    _block_end_time = adoDB.RecordGetTimeValue("block_end_time");
                    _recent_login_time = adoDB.RecordGetTimeValue("recent_login_time");
                    _recent_logout_time = adoDB.RecordGetTimeValue("recent_logout_time");
                    _gm_level = adoDB.RecordGetValue("gm_level");
                }
                else
                {
                    _strResult = "[gp_server_get_user] No Result!";
                }
                adoDB.RecordEnd();
            }
            catch (OdbcException e)
            {
                adoDB.RecordEnd();
                _strResult = "[gp_server_get_user] " + e.Message;
            }
        }
        public override string vGetName()
        {
            return "gp_server_get_user";
        }
    }

    public class DBGlobal_User_Login : QueryBase
    {
        //IN
        public int _platform_type;
        public string _encode_account_id;
        public ulong _user_db_key;
        
        public DBGlobal_User_Login() : base() { }

        public override void vRun(AdoDB adoDB)
        {
            try
            {
                QueryBuilder query = new QueryBuilder("call gp_auser_login(?,?,?)");
                query.SetInputParam("@p_platform_type", _platform_type);
                query.SetInputParam("@p_encode_account_id", _encode_account_id);
                query.SetInputParam("@p_user_db_key", _user_db_key);

                adoDB.ExecuteNoRecords(query);
            }
            catch (OdbcException e)
            {
                _strResult = "[gp_auser_login] " + e.Message;
            }
        }

        public override string vGetName()
        {
            return "gp_auser_login";
        }
    }

    public class DBGlobal_User_Logout : QueryBase
    {
        //IN
        public string _encode_account_id;
        public ulong _user_db_key;

        public DBGlobal_User_Logout() : base() { }

        public override void vRun(AdoDB adoDB)
        {
            try
            {
                QueryBuilder query = new QueryBuilder("call gp_auser_logout(?,?,?)");
                query.SetInputParam("@p_encode_account_id", _encode_account_id);
                query.SetInputParam("@p_user_db_key", _user_db_key);

                adoDB.ExecuteNoRecords(query);
            }
            catch (OdbcException e)
            {
                _strResult = "[gp_auser_logout] " + e.Message;
            }
        }

        public override string vGetName()
        {
            return "gp_auser_logout";
        }
    }

}

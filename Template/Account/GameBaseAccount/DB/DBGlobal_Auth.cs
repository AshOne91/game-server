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
        public ulong _encode_account_id;
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
                    _encode_account_id = adoDB.RecordGetValue("encode_account_id");
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
}

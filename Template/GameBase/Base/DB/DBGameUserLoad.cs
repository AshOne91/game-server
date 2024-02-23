using Service.DB;
using Service.Net;
using System;
using System.Collections.Generic;
using System.Text;

namespace GameBase.Template.GameBase
{
    public partial class DBGameUserLoad : QueryBaseValidate
    {
        //IN
        public UInt64 _user_db_key;
        public UInt64 _player_db_key;
        public string _encode_account_id;
        public Byte _gm_level;

        //Out
        public UserDB _userDB;

        public DBGameUserLoad(UserObject caller) : base(caller)
        {
            _userDB = GameBaseTemplateContext.CreateUserDB();
        }

        ~DBGameUserLoad()
        {

        }


        public override void vRun(AdoDB adoDB)
        {
            try
            {
                _userDB.LoadRun(adoDB, _user_db_key, _player_db_key);
            }
            catch (Exception Error)
            {
                _strResult = Error.Message;
            }
        }
        public override string vGetName() { return "DBGameUserLoad"; }
    }
}

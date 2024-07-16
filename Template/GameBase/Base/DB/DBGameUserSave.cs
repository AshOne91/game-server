﻿using Service.DB;
using System;
using System.Collections.Generic;
using System.Text;

namespace GameBase.Template.GameBase
{
    public partial class DBGameUserSave : QueryBase
    {
        public bool _isConnected;
        public UInt64 _user_db_key;
        public UInt64 _player_db_key;
        public UserDB _userDB;

        public UInt64 _uid;

        public DBGameUserSave()
        {
            _userDB = GameBaseTemplateContext.CreateUserDB();
        }
        ~DBGameUserSave()
        {

        }


        public override void vRun(AdoDB adoDB)
        {
            try
            {
                _userDB.SaveRun(adoDB, _user_db_key, _player_db_key);
            }
            catch (Exception Error)
            {
                _strResult = Error.Message;
            }
        }
        public override string vGetName() { return "DBGameUserSave"; }
    }
}

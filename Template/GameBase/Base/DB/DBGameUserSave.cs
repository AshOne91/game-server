using Service.DB;
using System;
using System.Collections.Generic;
using System.Text;

namespace GameBase.Template.GameBase
{
    public partial class DBGameUserSave : QueryBase
    {
        public bool _isConnected;
        public UInt64 _partitionKey_1;
        public UInt64 _partitionKey_2;
        public UserDB _userDB;

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
                _userDB.SaveRun(adoDB, _partitionKey_1, _partitionKey_2);
            }
            catch (Exception Error)
            {
                _strResult = Error.Message;
            }
        }
        public override string vGetName() { return "DBGameUserSave"; }
    }
}

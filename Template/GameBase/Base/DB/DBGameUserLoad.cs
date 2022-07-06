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
        public UInt64 _partitionKey_1;
        public UInt64 _partitionKey_2;
        public string _encode_account_id;
        public Byte _gm_level;

        //Out
        public UserDB _userDB;

        public DBGameUserLoad(UserObject caller) : base(caller)
        {
            _userDB = new UserDB();
        }

        ~DBGameUserLoad()
        {

        }


        public override void vRun(AdoDB adoDB)
        {
            try
            {
                _userDB.LoadRun(adoDB, _partitionKey_1, _partitionKey_2);
            }
            catch (Exception Error)
            {
                _strResult = Error.Message;
            }
        }
        public override string vGetName() { return "DBGameUserLoad"; }
    }
}

using System;
using System.Collections.Generic;
using Service.DB;

namespace GameBase.Template.GameBase
{
    public class GameBaseUserDB
    {
        virtual public void Copy(UserDB userSrc, bool isChanged)
        {

        }

        virtual public void LoadRun(AdoDB adoDB, UInt64 user_db_key, UInt64 player_db_key)
        {

        }

        virtual public void SaveRun(AdoDB adoDB, UInt64 user_db_key, UInt64 player_db_key)
        {

        }
    }
}

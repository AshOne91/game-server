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

        virtual public void LoadRun(AdoDB adoDB, UInt64 partitionKey_1, UInt64 partitionKey_2)
        {

        }

        virtual public void SaveRun(AdoDB adoDB, UInt64 partitionKey_1, UInt64 partitionKey_2)
        {

        }
    }
}

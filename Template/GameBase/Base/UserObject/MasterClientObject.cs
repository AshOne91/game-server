using System;
using System.Collections.Generic;
using System.Text;
using Service.Net;

namespace GameBase.Template.GameBase
{
    public partial class MasterClientObject : ImplObject
    {
        public MasterClientObject()
        {
            _objectID = (int)ObjectType.Master;
        }
    }
}

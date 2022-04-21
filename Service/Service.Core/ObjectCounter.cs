using System;
using System.Collections.Generic;
using System.Text;

namespace Service.Core
{
    public class ObjectCounter
    {
        public ObjectCounter()
        {

        }
        ~ObjectCounter()
        {

        }
        public long _ObjectCount = 0;
        public static ObjectCounter Default = new ObjectCounter();
    }
}

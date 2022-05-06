using System;
using System.Collections.Generic;
using System.Text;

namespace Service.Core
{
    public class AsyncTaskProcedure
    {
        public virtual bool Create()
        {
            return false;
        }
        public virtual void Execute(AsyncTaskObject task) { }
    }
}

using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;

namespace Service.Core
{
    public class FatalException : Exception
    {
        public FatalException(string msg) : base(msg)
        {
            Debugger.Break();
        }
    }
}

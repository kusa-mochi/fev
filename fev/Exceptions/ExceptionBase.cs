using System;
using System.Collections.Generic;
using System.Text;

using fev.Common;

namespace fev.Exceptions
{
    public class ExceptionBase : Exception
    {
        protected LogManager _logManager = LogManager.GetInstance();

        public ExceptionBase() : base()
        {

        }

        public ExceptionBase(string message) : base(message)
        {

        }
    }
}

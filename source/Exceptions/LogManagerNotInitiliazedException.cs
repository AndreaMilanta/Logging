using System;

namespace Logging.Exceptions
{
    class LogManagerNotInitiliazedException : Exception
    {
        public override string ToString()
        {
            return "Logger Manager has not yet been initialized.";
        }
    }
}

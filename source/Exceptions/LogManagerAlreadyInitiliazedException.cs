using System;

namespace Logging.Exceptions
{
    class LogManagerAlreadyInitiliazedException : Exception
    {
        public override string ToString()
        {
            return "Logger Manager has already been initialized.";
        }
    }
}

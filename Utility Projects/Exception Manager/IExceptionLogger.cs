using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Bardez.Projects.ExceptionHandler
{
    public interface IExceptionLogger
    {
        void LogException(Exception Ex);
        //void LogException(String SystemInfo, String ExceptionDetails);
    }
}

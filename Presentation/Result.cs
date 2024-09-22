using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentInfoSys.Presentation
{
    public class Result
    {
        public bool Success { get; }
        public string Message { get; }

        public Result(bool success, string message = "")
        {
            Success = success;  // did it pass validation method
            Message = message;  // if not, what was error message
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AwsApiToLambdaLib
{
    public class ExecutionError
    {
        public string Error { get; set; }
        public string StackTrace { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AwsApiToLambdaLib
{
    static public partial class Handler
    {
        static public object Func(object req)
        {
            return "Hello";
        }

    }
}

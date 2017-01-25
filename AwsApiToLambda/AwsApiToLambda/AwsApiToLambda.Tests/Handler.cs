using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AwsApiToLambda.Tests
{
    static public class Handler
    {
        static public FirstObjResponse Func(FirstObjRequest req)
        {
            return new FirstObjResponse();
        }

        static public SecondObjResponse Func(SecondObjRequest req)
        {
            return new SecondObjResponse();
        }
    }
}

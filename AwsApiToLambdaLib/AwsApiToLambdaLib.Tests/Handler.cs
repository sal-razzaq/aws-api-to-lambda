using AwsApiToLambdaLib.Tests;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AwsApiToLambdaLib
{
    static public partial class Handler
    {
        static public GreetingResponse Func(GreetingRequest request)
        {
            return new GreetingResponse()
            {
                Greeting = "Hello " + request?.Name
            };
        }
    }
}

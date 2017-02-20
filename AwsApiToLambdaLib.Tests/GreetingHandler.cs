using AwsApiToLambdaLib.Tests;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AwsApiToLambdaLib.Tests
{
    public class GreetingHandler
    {
        public GreetingResponse Process(GreetingRequest request, ICallContext requestContext)
        {
            return new GreetingResponse()
            {
                Greeting = "Hello " + request?.Name
            };
        }
    }
}

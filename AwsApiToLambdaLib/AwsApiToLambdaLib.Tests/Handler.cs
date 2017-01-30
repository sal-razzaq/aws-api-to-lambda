using AwsApiToLambdaLib.Tests;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AwsApiToLambdaLib.Tests
{
    public class Handler
    {
        //public GreetingResponse Execute(GreetingRequest request)
        public GreetingResponse Process(GreetingRequest request)
        {
            return new GreetingResponse()
            {
                Greeting = "Hello " + request?.Name
            };
        }

        public GreetingResponse Process(String request)
        {
            //return new GreetingResponse()
            //{
            //    Greeting = "Hello " + request?.Name
            //};
            return null;
        }
    }
}

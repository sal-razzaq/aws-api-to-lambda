﻿using Amazon.Runtime;
using AwsApiToLambdaLib;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GreetingExample
{
    public class GreetingHandler
    {
        public GreetingResponse Process(GreetingRequest request, ICallContext callContext)
        {
            return new GreetingResponse()
            {
                Greeting = "Hello " + request?.Name,
                EchoGreetingRequest = request,
                EchoCallContext = callContext
            };
        }
    }
}

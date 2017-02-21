using Amazon.Runtime;
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
        public GreetingResponse Hello(GreetingRequest request, ICallContext callContext)
        {
            return new GreetingResponse()
            {
                Greeting = "Hello " + request?.Name
            };
        }

        public GreetingResponse Bye(GreetingRequest request, ICallContext callContext)
        {
            string name;
            callContext.ApiGatewayInput.@params.querystring.TryGetValue("name", out name);
            return new GreetingResponse()
            {
                Greeting = "Bye " + (name != null ? name : string.Empty)
            };
        }
    }
}

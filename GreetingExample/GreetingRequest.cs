using AwsApiToLambdaLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GreetingExample
{
    public class GreetingRequest : ApiGatewayInput
    {
        public string Name { get; set; }
    }
}

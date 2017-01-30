using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AwsApiToLambdaLib.Tests
{
    public class MockApiGatewayRequest
    {
        public string classType { get; set; }
        public string methodName { get; set; }
        public string methodParamType { get; set; }
        public string body { get; set; }
    }
}

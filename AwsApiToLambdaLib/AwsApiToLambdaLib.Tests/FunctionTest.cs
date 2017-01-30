using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Xunit;
using Amazon.Lambda.Core;
using Amazon.Lambda.TestUtilities;

using AwsApiToLambdaLib;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.IO;
using System.Text;
using AwsApiToLambdaLib.Tests;

namespace AwsApiToLambdaLib.Tests
{
    public class FunctionTest
    {
        [Fact]
        public void BasicTest()
        {
            var function = new Function();
            var context = new TestLambdaContext();
            MockApiGatewayRequest apiGatewayRequest = new MockApiGatewayRequest()
            {
                classType = "AwsApiToLambdaLib.Tests.Handler, AwsApiToLambdaLib.Tests",
                methodName = "Process",
                methodParamType = "AwsApiToLambdaLib.Tests.GreetingRequest, AwsApiToLambdaLib.Tests",
                body = JsonConvert.SerializeObject(new { Name = "Beavis" })
            };
            var response = function.FunctionHandler(JsonConvert.SerializeObject(apiGatewayRequest), context);
            var responseObj = JsonConvert.DeserializeObject<GreetingResponse>(response);
            Assert.Equal("Hello Beavis", responseObj.Greeting);

            // call again - this time the cached method will be used
            apiGatewayRequest.body = JsonConvert.SerializeObject(new { Name = "Butthead" });
            response = function.FunctionHandler(JsonConvert.SerializeObject(apiGatewayRequest), context);
            responseObj = JsonConvert.DeserializeObject<GreetingResponse>(response);
            Assert.Equal("Hello Butthead", responseObj.Greeting);
        }
    }
}

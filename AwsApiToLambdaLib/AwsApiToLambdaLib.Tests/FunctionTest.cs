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
            var apiGatewayRequest = new ApiGatewayInput()
            {
                class_type = "AwsApiToLambdaLib.Tests.Handler, AwsApiToLambdaLib.Tests",
                method_name = "Process",
                method_param_type = "AwsApiToLambdaLib.Tests.GreetingRequest, AwsApiToLambdaLib.Tests",
                body_json = JsonConvert.SerializeObject(new { Name = "Beavis" })
            };
            var response = function.FunctionHandler(apiGatewayRequest, context);
            var responseObj = JsonConvert.DeserializeObject<GreetingResponse>(response);
            Assert.Equal("Hello Beavis", responseObj.Greeting);

            // call again - this time the cached method will be used
            apiGatewayRequest.body_json = JsonConvert.SerializeObject(new { Name = "Butthead" });
            response = function.FunctionHandler(apiGatewayRequest, context);
            responseObj = JsonConvert.DeserializeObject<GreetingResponse>(response);
            Assert.Equal("Hello Butthead", responseObj.Greeting);
        }
    }
}

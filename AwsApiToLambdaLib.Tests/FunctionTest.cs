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
            var function = new AwsApiToLambdaLibFunction();
            var context = new TestLambdaContext();
            var apiGatewayRequest = new ApiGatewayInput()
            {
                class_type = "AwsApiToLambdaLib.Tests.GreetingHandler, AwsApiToLambdaLib.Tests",
                method_name = "Process",
                method_param_type = "AwsApiToLambdaLib.Tests.GreetingRequest, AwsApiToLambdaLib.Tests",
                body_json = JObject.FromObject(new GreetingRequest { Name = "Beavis" })
            };
            var responseObj = (GreetingResponse) function.FunctionHandler(apiGatewayRequest, context);
            Assert.Equal("Hello Beavis", responseObj.Greeting);

            // call again - this time the cached method will be used
            apiGatewayRequest.body_json = JObject.FromObject(new GreetingRequest { Name = "Butthead" });
            responseObj = (GreetingResponse) function.FunctionHandler(apiGatewayRequest, context);
            Assert.Equal("Hello Butthead", responseObj.Greeting);
        }

        [Fact]
        public void FullApiGatewayInputTest()
        {
            var function = new AwsApiToLambdaLibFunction();
            var context = new TestLambdaContext();
            var inputJson = File.ReadAllText("apigateway-input.json");
            var responseObj = (GreetingResponse) function.FunctionHandler(JsonConvert.DeserializeObject<ApiGatewayInput>(inputJson), context);
            Assert.Equal("Hello Joe", responseObj.Greeting);
        }

        [Fact]
        public void NullApiGatewayInputTest()
        {
            var function = new AwsApiToLambdaLibFunction();
            var context = new TestLambdaContext();
            dynamic responseObj = function.FunctionHandler(null, context);
            Assert.True(responseObj.Error != null);
        }

        [Fact]
        public void EmptyApiGatewayInputTest()
        {
            var function = new AwsApiToLambdaLibFunction();
            var context = new TestLambdaContext();
            var inputJson = File.ReadAllText("apigateway-input-empty.json");
            dynamic responseObj = function.FunctionHandler(JsonConvert.DeserializeObject<ApiGatewayInput>(inputJson), context);
            Assert.True(responseObj.Error != null && responseObj.Error.Contains("class-type"));
        }

        [Fact]
        public void NoJsonBodyApiGatewayInputTest()
        {
            var function = new AwsApiToLambdaLibFunction();
            var context = new TestLambdaContext();
            var inputJson = File.ReadAllText("apigateway-input-no-json-body.json");
            var responseObj = (GreetingResponse) function.FunctionHandler(JsonConvert.DeserializeObject<ApiGatewayInput>(inputJson), context);
            Assert.Equal("Hello ", responseObj.Greeting);
        }
    }
}

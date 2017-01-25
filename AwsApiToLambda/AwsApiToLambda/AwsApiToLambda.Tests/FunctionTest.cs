using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Xunit;
using Amazon.Lambda.Core;
using Amazon.Lambda.TestUtilities;

using AwsApiToLambda;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace AwsApiToLambda.Tests
{
    public class FunctionTest
    {
        [Fact]
        public void TestToUpperFunction()
        {

            // Invoke the lambda function and confirm the string was upper cased.
            var function = new Function();
            var context = new TestLambdaContext();
            var upperCase = function.FunctionHandler("hello world", context);

            Assert.Equal("HELLO WORLD", upperCase);
        }

        [Fact]
        public void TestTypeBasedRouting()
        {
            string typeString = "AwsApiToLambda.Tests.SecondObjRequest, AwsApiToLambda.Tests";
            Type type = Type.GetType(typeString);
            dynamic obj = Activator.CreateInstance(type);
            
            //Amazon.Lambda.Serialization.Json.JsonSerializer
            //Newtonsoft.Json.Linq.JObject
            Handler.Func(obj);
        }

        [Fact]
        public void TestJsonToObject()
        {
            string typeString = "AwsApiToLambda.Tests.FirstObjRequest, AwsApiToLambda.Tests";
            Type type = Type.GetType(typeString);
            string json = "{\"val\" : 1}";


            dynamic obj = JsonConvert.DeserializeObject(json, type);
            Handler.Func(obj);
        }

        [Fact]
        public void TestFullTest()
        {
            string jsonRequest = "{\"requestObjectType\": \"AwsApiToLambda.Tests.SecondObjRequest, AwsApiToLambda.Tests\",\"body\": \"{val : 1}\"}";
            dynamic jsonRequestObj = JObject.Parse(jsonRequest);
            string typeString = jsonRequestObj.requestObjectType;
            //string typeString = "AwsApiToLambda.Tests.FirstObjRequest, AwsApiToLambda.Tests";
            Type type = Type.GetType(typeString);
            //string json = "{\"val\" : 1}";
            string json = jsonRequestObj.body;
            dynamic obj = JsonConvert.DeserializeObject(json, type);
            object ret = Handler.Func(obj);
        }
    }
}

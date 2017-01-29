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

namespace AwsApiToLambdaLib.Tests
{
    public class FunctionTest
    {
        [Fact]
        public void TestToUpperFunction()
        {
            // Invoke the lambda function and confirm the string was upper cased.
            var function = new Function();
            var context = new TestLambdaContext();
            string input = "{\"msg\": \"hello world\"}";
            var response = function.FunctionHandler(input, context);
            //Assert.Equal("HELLO WORLD", upperCase);
        }

        
        //[Fact]
        //public void TestTypeBasedRouting()
        //{
        //    string typeString = "AwsApiToLambda.Tests.SecondObjRequest, AwsApiToLambda.Tests";
        //    Type type = Type.GetType(typeString);
        //    dynamic obj = Activator.CreateInstance(type);
            
        //    //Amazon.Lambda.Serialization.Json.JsonSerializer
        //    //Newtonsoft.Json.Linq.JObject
        //    Handler.Func(obj);
        //}

        //[Fact]
        //public void TestJsonToObject()
        //{
        //    string typeString = "AwsApiToLambda.Tests.FirstObjRequest, AwsApiToLambda.Tests";
        //    Type type = Type.GetType(typeString);
        //    string json = "{\"val\" : 1}";


        //    dynamic obj = JsonConvert.DeserializeObject(json, type);
        //    Handler.Func(obj);
        //}

        //[Fact]
        //public void TestFull()
        //{
        //    string jsonRequest = "{\"requestType\": \"AwsApiToLambda.Tests.SecondObjRequest, AwsApiToLambda.Tests\",\"body\": \"{val : 1}\"}";
        //    dynamic jsonRequestObj = JObject.Parse(jsonRequest);
        //    string typeString = jsonRequestObj.requestType;
        //    //string typeString = "AwsApiToLambda.Tests.FirstObjRequest, AwsApiToLambda.Tests";
        //    Type type = Type.GetType(typeString);
        //    //string json = "{\"val\" : 1}";
        //    string json = jsonRequestObj.body;
        //    dynamic obj = JsonConvert.DeserializeObject(json, type);
        //    object ret = Handler.Func(obj);
        //}

        //[Fact]
        //public void TestFullFromFile()
        //{
        //    string jsonRequest = File.ReadAllText("SampleAPIGatewayData.json");
        //    dynamic jsonRequestObj = JObject.Parse(jsonRequest);
        //    string typeString = jsonRequestObj.requestType;
        //    //string typeString = "AwsApiToLambda.Tests.FirstObjRequest, AwsApiToLambda.Tests";
        //    Type type = Type.GetType(typeString);
        //    //string json = "{\"val\" : 1}";
        //    string json = jsonRequestObj.body.ToString();
        //    dynamic obj = JsonConvert.DeserializeObject(json, type);
        //    object ret = Handler.Func(obj);
        //}
    }
}

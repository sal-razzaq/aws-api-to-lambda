using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Amazon.Lambda.Core;
using Amazon.Lambda.Serialization;
using Newtonsoft.Json.Linq;
using System.IO;
using Newtonsoft.Json;
using System.Text;

// Assembly attribute to enable the Lambda function's JSON input to be converted into a .NET class.
[assembly: LambdaSerializerAttribute(typeof(Amazon.Lambda.Serialization.Json.JsonSerializer))]

namespace AwsApiToLambdaLib
{
    public class Function
    {

        /// <summary>
        /// A simple function that takes a string and does a ToUpper
        ///  if you use a Lambda function as a mobile application backend, you are invoking it synchronously. Your output data type will be serialized into JSON.
        /// </summary>
        /// <param name="input"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        public String FunctionHandler(String input, ILambdaContext context)
        {
            try
            {
                dynamic inputJsonObj = JObject.Parse(input);
                string typeString = inputJsonObj.requestType;
                if (String.IsNullOrWhiteSpace(typeString))
                {
                    throw new Exception("requestType not specified in input.");
                }
                Type type = Type.GetType(typeString);
                if (type == null)
                {
                    throw new Exception($"Unable to resolve requestType: {typeString}");
                }
                string jsonBody = inputJsonObj.body;
                if (string.IsNullOrWhiteSpace(jsonBody))
                {
                    throw new Exception("request body is empty.");
                }
                dynamic inputObj = JsonConvert.DeserializeObject(jsonBody, type);
                if (inputObj == null)
                {
                    throw new Exception("Request object deserialized from request body is null.");
                }
                return JsonConvert.SerializeObject(Handler.Func(inputObj));
            }
            catch (Exception ex)
            {
                return JsonConvert.SerializeObject(
                    new
                    {
                        Error = ex.Message,
                        StackTrace = ex.StackTrace
                    });
            }
        }
    }
}

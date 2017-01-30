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
using System.Reflection;

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
                JObject inputJsonObj = JObject.Parse(input);
                Type classType = ResolveType(inputJsonObj, "classType");
                string methodName = GetPropertyStringValue(inputJsonObj, "methodName");
                Type methodParamType = ResolveType(inputJsonObj, "methodParamType");
                string jsonBody = GetPropertyStringValue(inputJsonObj, "body");
                if (string.IsNullOrWhiteSpace(jsonBody))
                {
                    throw new Exception("request body is empty.");
                }
                dynamic requestData = JsonConvert.DeserializeObject(jsonBody, methodParamType);
                if (requestData == null)
                {
                    throw new Exception("Request object deserialized from request body is null.");
                }
                var handlerClass = Activator.CreateInstance(classType);

                var methodInfo = GetMethodWithCorrectParam(classType, methodName, methodParamType);
                if (methodInfo == null)
                {
                    throw new Exception($"Request Handler not found. Expected Method: {methodName} on Class: {classType} which accepts exactly one argument of Type: {methodParamType}.");
                }
                var result = methodInfo.Invoke(handlerClass, new object[] { requestData });
                return JsonConvert.SerializeObject(result);
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

        MethodInfo GetMethodWithCorrectParam(Type classType, String methodName, Type methodParamType)
        {
            MethodInfo[] methodInfos = classType.GetMethods();
            foreach (var mi in methodInfos)
            {
                if (mi.Name == methodName)
                {
                    var methodParam = mi.GetParameters();
                    if (methodParam.Length == 1)
                    {
                        var paramType = methodParam[0].ParameterType;
                        if (paramType == methodParamType)
                        {
                            return mi;
                        }
                    }
                }
            }
            return null;
        }
        String GetPropertyStringValue(JObject inputJsonObj, string name)

        {
            JToken jToken;
            string val = null;
            if (inputJsonObj.TryGetValue(name, out jToken))
            {
                val = jToken.ToString();
            }
            return val;
        }

        Type ResolveType(JObject inputJsonObj, string name)
        {
            var typeString = GetPropertyStringValue(inputJsonObj, name);
            if (String.IsNullOrWhiteSpace(typeString))
            {
                throw new Exception($"{name} not specified in input.");
            }
            Type type = Type.GetType(typeString);
            if (type == null)
            {
                throw new Exception($"Unable to resolve {name}: {typeString}");
            }
            return type;
        }
    }
}

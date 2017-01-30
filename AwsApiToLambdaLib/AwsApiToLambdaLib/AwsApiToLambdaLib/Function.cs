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
using System.Collections.Concurrent;

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
        public String FunctionHandler(ApiGatewayInput input, ILambdaContext context)
        {
            try
            {
                Type classType = ResolveType(input.class_type);
                string methodName = input.method_name;
                Type methodParamType = ResolveType(input.method_param_type);
                if (string.IsNullOrWhiteSpace(input.body_json))
                {
                    throw new Exception("request body is empty.");
                }
                dynamic requestData = JsonConvert.DeserializeObject(input.body_json, methodParamType);
                if (requestData == null)
                {
                    throw new Exception("Request object deserialized from request body is null.");
                }
                var handlerClass = Activator.CreateInstance(classType);

                var methodInfo = GetMethodWithCorrectParam(classType, methodName, methodParamType);
                if (methodInfo == null)
                {
                    throw new Exception($"Request Handler not found. Expected Method: {methodName} on Class: {classType} which accepts two arguments of Types: {methodParamType} and {typeof(IRequestContext)}.");
                }
                RequestContext requestContext = new RequestContext()
                {
                    LambdaContext = context
                };
                var result = methodInfo.Invoke(handlerClass, new object[] { requestData, requestContext });
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

        ConcurrentDictionary<string, MethodInfo> _methodCache = new ConcurrentDictionary<string, MethodInfo>();
        MethodInfo GetMethodWithCorrectParam(Type classType, String methodName, Type methodParamType)
        {
            string key = classType + ";" + methodName + ";" + methodParamType;
            MethodInfo methodInfoLookup = null;
            if (_methodCache.TryGetValue(key, out methodInfoLookup))
                return methodInfoLookup;

            MethodInfo[] methodInfos = classType.GetMethods();
            foreach (var mi in methodInfos)
            {
                if (mi.Name == methodName)
                {
                    var methodParam = mi.GetParameters();
                    if (methodParam.Length == 2)
                    {
                        if (methodParam[0].ParameterType == methodParamType 
                            && methodParam[1].ParameterType == typeof(IRequestContext))
                        {
                            if (!_methodCache.TryGetValue(key, out methodInfoLookup))
                            {
                                _methodCache[key] = mi;
                            }
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

        Type ResolveType(string typeString)
        {
            Type type = Type.GetType(typeString);
            if (type == null)
            {
                throw new Exception($"Unable to resolve type: {typeString}");
            }
            return type;
        }
    }
}

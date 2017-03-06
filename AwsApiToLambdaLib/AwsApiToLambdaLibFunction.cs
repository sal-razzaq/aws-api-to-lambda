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
    public class AwsApiToLambdaLibFunction
    {
        /// <summary>
        /// Main function that routes input to configured handlers
        ///     This class should be specified as the handler function for the Aws Lambda
        ///     The routing information is passed from the API Gateway to this method
        /// </summary>
        /// <param name="input"></param>
        /// <param name="context"></param>
        /// <returns>serialized json corresponding to the object returned by the handler method</returns>
        public object FunctionHandler(ApiGatewayInput input, ILambdaContext context)
        {
            try
            {
                if (input == null)
                {
                    throw new Exception("input is null." + CONFIG_MSG);
                }
                Type classType = ResolveType(input.class_type, "class-type");
                string methodName = input.method_name;
                Type methodParamType = ResolveType(input.method_param_type, "method-param-type");
                dynamic requestData = null;
                if (input.body_json != null)
                    requestData = input.body_json.ToObject(methodParamType);
                var methodInfo = GetMethodWithCorrectParam(classType, methodName, methodParamType);
                if (methodInfo == null)
                {
                    throw new Exception($"Request Handler method not found. Expected method: {methodName} on Class: {classType} which accepts two arguments of Types: {methodParamType} and {typeof(ICallContext)}.");
                }
                CallContext requestContext = new CallContext()
                {
                    LambdaContext = context,
                    ApiGatewayInput = input
                };

                var handlerClass = Activator.CreateInstance(classType);
                var result = methodInfo.Invoke(handlerClass, new object[] { requestData, requestContext });
                return result;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"{ex.Message}\n{ex.StackTrace}");
                return (
                    new ExecutionError
                    {
                        Error = ex.Message,
                        StackTrace = ex.StackTrace
                    });
            }
        }

        const string CONFIG_MSG = "Aws API Gateway not configured. Did you configure the application/json mapping template under 'Integration Request'?";
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
                            && methodParam[1].ParameterType == typeof(ICallContext))
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

        Type ResolveType(string typeString, string name)
        {
            if (String.IsNullOrWhiteSpace(typeString))
                throw new Exception($"{name} not specified in input. " + CONFIG_MSG);
            Type type = Type.GetType(typeString);
            if (type == null)
            {
                throw new Exception($"{name}: Unable to resolve type: {typeString}");
            }
            return type;
        }
    }
}

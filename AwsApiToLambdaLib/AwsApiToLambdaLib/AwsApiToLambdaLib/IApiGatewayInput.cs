using System.Collections.Generic;

namespace AwsApiToLambdaLib
{
    public interface IApiGatewayInput
    {
        dynamic body_json { get; }
        string class_type { get; }
        string method_name { get; }
        string method_param_type { get; }
        IParams @params { get; }
        Dictionary<string, string> stage_variables { get; }
        Dictionary<string, string> context { get; }
    }
}
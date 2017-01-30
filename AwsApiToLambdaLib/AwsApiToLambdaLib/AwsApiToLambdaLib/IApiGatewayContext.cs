using System.Collections.Generic;

namespace AwsApiToLambdaLib
{
    public interface IApiGatewayContext
    {
        IParams @params { get; }
        Dictionary<string, string> stage_variables { get; }
        Dictionary<string, string> context { get; }
    }
}
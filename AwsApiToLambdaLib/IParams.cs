using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AwsApiToLambdaLib
{
    /// <summary>
    /// Parameters (key-value pairs) passed from API Gateway to Lambda by virtue of the mapping template in the Integration request
    /// </summary>
    public interface IParams
    {
        Dictionary<string, string> path { get; }
        Dictionary<string, string> querystring { get; }
        Dictionary<string, string> header { get; }
    }
}

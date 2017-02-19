using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AwsApiToLambdaLib
{
    /// <summary>
    /// Parameters (key-value pairs) passed from API Gateway to Lambda by virtue of the mapping template in the Integration request
    /// </summary>
    public class Params : IParams
    {
        public Dictionary<string, string> path { get; set; }
        public Dictionary<string, string> querystring { get; set; }
        public Dictionary<string, string> header { get; set; }
    }
}

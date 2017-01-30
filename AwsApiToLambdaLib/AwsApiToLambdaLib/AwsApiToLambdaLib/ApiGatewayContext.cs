using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AwsApiToLambdaLib
{
    public class ApiGatewayContext : IApiGatewayContext
    {
        public IParams @params { get; set; }
        public Dictionary<string, string> stage_variables { get; set; }
        public Dictionary<string, string> context { get; set; }
    }
}

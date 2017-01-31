using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AwsApiToLambdaLib
{
    public class Params : IParams
    {
        public Dictionary<string, string> path { get; set; }
        public Dictionary<string, string> querystring { get; set; }
        public Dictionary<string, string> header { get; set; }
    }
}

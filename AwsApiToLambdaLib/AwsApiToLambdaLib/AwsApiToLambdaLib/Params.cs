using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AwsApiToLambdaLib
{
    public class Params
    {
        public Dictionary<string, string> path;
        public Dictionary<string, string> querystring;
        public Dictionary<string, string> header;
        public Dictionary<string, string> stage_variables;
        public Dictionary<string, string> context;
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AwsApiToLambdaLib
{
    public class ApiGatewayInput
    {
        public string class_type { get; set; }
        public string method_name { get; set; }
        public string method_param_type { get; set; }
        public String body_json { get; set; }
        public Params @params {get; set;}
    }
}

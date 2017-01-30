using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AwsApiToLambdaLib
{
    public class ApiGatewayInput
    {
        [JsonProperty("class-type")]
        public string class_type { get; set; }
        [JsonProperty("method-name")]
        public string method_name { get; set; }
        [JsonProperty("method-param-type")]
        public string method_param_type { get; set; }
        [JsonProperty("body-json")]
        public dynamic body_json { get; set; }
        public Params @params {get; set;}
        [JsonProperty("stage-variables")]
        public Dictionary<string, string> stage_variables;
        public Dictionary<string, string> context;
    }
}

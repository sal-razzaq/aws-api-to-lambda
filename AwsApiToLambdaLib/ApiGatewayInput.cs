﻿using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AwsApiToLambdaLib
{
    /// <summary>
    /// Base class with data members that accepts all data passed from the Aws Api Gateway via the mapping template specified in the "Integration Request"
    /// </summary>
    public class ApiGatewayInput : IApiGatewayInput
    {
        public ApiGatewayInput()
        {
            this.@params = new Params();
        }
        
        [JsonProperty("class-type")]
        public string class_type { get; set; }
        [JsonProperty("method-name")]
        public string method_name { get; set; }
        [JsonProperty("method-param-type")]
        public string method_param_type { get; set; }
        [JsonProperty("body-json")]
        public dynamic body_json { get; set; }
        public IParams @params {get; set;}
        [JsonProperty("stage-variables")]
        public Dictionary<string, string> stage_variables { get; set; }
        public Dictionary<string, string> context { get; set; }
    }
}
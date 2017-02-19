using System.Collections.Generic;

namespace AwsApiToLambdaLib
{
    /// <summary>
    /// Represents data passed by Aws Api Gateway to the lambda handler
    ///     by virtue of the mapping template specified in the Integration Request
    ///     
    /// </summary>
    public interface IApiGatewayInput
    {
        /// <summary>
        /// Name of the .Net class that should handle this request
        /// </summary>
        string class_type { get; }
        /// <summary>
        /// Name of the method in the class_type that should process this request
        /// </summary>
        string method_name { get; }
        /// <summary>
        /// .Net type of request passed to the method_name
        /// </summary>
        string method_param_type { get; }
        /// <summary>
        /// json document passed by Aws Api Gateway passed to method_name
        ///     deserializes to method_param_type
        /// </summary>
        dynamic body_json { get; }
        /// <summary>
        /// path, querystring, header key value pairs passed by Aws Api Gateway
        /// </summary>
        IParams @params { get; }
        /// <summary>
        /// Api Gateway stage variables
        /// </summary>
        Dictionary<string, string> stage_variables { get; }
        /// <summary>
        /// context of the Api Gateway call (account-id, app-id, http-method, etc.)
        /// </summary>
        Dictionary<string, string> context { get; }
    }
}

 
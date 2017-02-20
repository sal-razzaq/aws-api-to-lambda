using Amazon.Lambda.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AwsApiToLambdaLib
{
    /// <summary>
    /// Provides context for each call from AWS API Gateway to AWS Lambda
    /// </summary>
    public interface ICallContext
    {
        /// <summary>
        /// LambaContext as passed by the Aws Lambda runtime
        /// </summary>
        ILambdaContext LambdaContext { get; }
        /// <summary>
        /// API Gateway context of the call (as mapped in the Integration request)
        /// </summary>
        IApiGatewayInput ApiGatewayInput { get; }
    }
}

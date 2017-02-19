using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Amazon.Lambda.Core;

namespace AwsApiToLambdaLib
{
    /// <summary>
    /// Provides context for each call from AWS API Gateway to AWS Lambda
    /// </summary>
    public class RequestContext : IRequestContext
    {
        public ILambdaContext LambdaContext { get; set; }

        public IApiGatewayInput ApiGatewayInput { get; set; }
    }
}

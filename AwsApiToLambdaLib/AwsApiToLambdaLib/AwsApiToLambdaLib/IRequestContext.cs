using Amazon.Lambda.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AwsApiToLambdaLib
{
    public interface IRequestContext 
    {
        ILambdaContext LambdaContext { get; }

        // pass in headers from API Gateway request
        ApiGatewayInput ApiGatewayInput { get; }
    }
}

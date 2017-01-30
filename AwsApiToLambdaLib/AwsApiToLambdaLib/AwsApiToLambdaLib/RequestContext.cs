using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Amazon.Lambda.Core;

namespace AwsApiToLambdaLib
{
    public class RequestContext : IRequestContext
    {
        public ILambdaContext LambdaContext { get; set; }
    }
}

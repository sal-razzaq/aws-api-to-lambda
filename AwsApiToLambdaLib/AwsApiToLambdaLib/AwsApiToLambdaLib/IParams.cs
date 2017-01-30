using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AwsApiToLambdaLib
{
    public interface IParams
    {
        Dictionary<string, string> path { get; }
        Dictionary<string, string> querystring { get; }
        Dictionary<string, string> header { get; }
    }
}

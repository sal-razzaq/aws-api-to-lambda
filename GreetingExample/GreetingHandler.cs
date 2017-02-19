using Amazon.Runtime;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GreetingExample
{
    public class GreetingHandler
    {
        public GreetingResponse Process(GreetingRequest request, IRequestContext requestContext)
        {
            return new GreetingResponse()
            {
                Greeting = "Hello " + request?.Name
            };
        }
    }
}

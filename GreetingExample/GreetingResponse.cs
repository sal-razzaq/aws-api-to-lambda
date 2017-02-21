using AwsApiToLambdaLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GreetingExample
{
  
    public class GreetingResponse
    {
        public string Greeting { get; set; }

        public string Error { get; set; }

        public string StackTrace { get; set; }

    }
   
}
    
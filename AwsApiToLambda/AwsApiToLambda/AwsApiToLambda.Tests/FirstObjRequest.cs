using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AwsApiToLambda.Tests
{
    public class FirstObjRequest
    {
        public string Val { get; set; }

        public string AppId { get; set; }

        public DateTime EventTime { get; set; }

        public Dictionary<string, string> json { get; set; }
    }
}

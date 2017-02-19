# aws-api-to-lambda

SUMMARY

- A library that forwards request received by Aws Api Gateway to a specified .Net class in an Aws Lambda function for processing. 

- This allows a single lambda function to handle different request types based on the Api Gateway endpoint configuration.

- The forwarding information in provided declaratively in the AWS Api Gateway configuration.

- The library takes care of deserializing requests into .Net request objects for processing by the Aws lambda function request handlers.


EXAMPLE 1 - GreetingExample

Objective: This lambda function will receive a request from AWS API Gateway and return a greeting.

1. Create a new project
AWS lambda | AWS lambda Project (.Net Core) | Empty function

2. Add project reference to "AwsApiToLambdaLib"


3. Edit "aws-lambda-tools-defaults.json"

Change 
"function-handler" : "GreetingExample::GreetingExample.Function::FunctionHandler"
to
"function-handler" : "AwsApiToLambdaLib::AwsApiToLambdaLib.Function::FunctionHandler"

4. Delete "Function.cs" file from GreetingExample Project

5. Create the following three classes that will handle request (request class, response class and the handler class)

// GreetingRequest.cs
public class GreetingRequest
{
    public string Name { get; set; }
}


// GreetingResponse.cs
public class GreetingResponse
{
    public string Greeting { get; set; }

    public string Error { get; set; }

    public string StackTrace { get; set; }
}


// GreetingHandler.cs
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

6. Build and deploy GreetingExample to Aws lambda


7. Configure API Gateway to call GreetingExample
# AWS-api-to-lambda

== SUMMARY ==

- Problem: I don't want to write multiple AWS Lambda functions to handle different web requests from AWS Api Gateway.

- This framework routes requests received by AWS Api Gateway to a specified .Net class in a single AWS Lambda function for processing.

- This framework allows a single lambda function to handle different request types based on the Api Gateway endpoint configuration.
The request forwarding/processing information in provided declaratively in the AWS Api Gateway configuration using templates.

- The framework also takes care of deserializing requests into .Net request objects for processing by the AWS lambda function request handlers.


== EXAMPLE - GreetingExample ==

Objective: 

This lambda function will receive a request from AWS API Gateway and return
two types of greetings: Hello and Bye.

In API Gateway we will integrate with this lambda function.
We will POST a "name" for "Hello" and GET a "Bye" by passing "name" as a querystring parameter.

1. Create a new project
AWS lambda | AWS lambda Project (.Net Core) | Empty function


2. Add project reference to "AWSApiToLambdaLib"


3. Edit "AWS-lambda-tools-defaults.json"

Change 
"function-handler" : "GreetingExample::GreetingExample.Function::FunctionHandler"
to
"function-handler" : "AWSApiToLambdaLib::AWSApiToLambdaLib.Function::FunctionHandler"


4. Delete "Function.cs" file from GreetingExample Project


5. Create the following three classes that will handle the request (request class, response class and a  request handler class)

// GreetingRequest.cs
public class GreetingRequest : ApiGatewayInput
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
    public GreetingResponse Hello(GreetingRequest request, ICallContext callContext)
    {
        return new GreetingResponse()
        {
            Greeting = "Hello " + request?.Name
        };
    }

    public GreetingResponse Bye(GreetingRequest request, ICallContext callContext)
    {
        string name;
        callContext.ApiGatewayInput.@params.querystring.TryGetValue("name", out name);
        return new GreetingResponse()
        {
            Greeting = "Bye " + (name != null ? name : string.Empty)
        };
    }
}


6. Build and deploy GreetingExample to AWS lambda

Make sure to add "Amazon.Lambda.Core" nuget package to GreetingExample project.

Also add "AWSSDK.Core 3.3.8.1" or later nuget package to GreetingExample project.


7. Test the Lambda directly with the following request to the "Hello" method. 
We will later POST to this method from the API Gateway.

{
	"class-type": "GreetingExample.GreetingHandler, GreetingExample",
	"method-name": "Hello",
	"method-param-type": "GreetingExample.GreetingRequest, GreetingExample",
	"body-json": {
		"Name": "Joe"
	}
}

It should return the following.

{"Greeting":"Hello Joe","Error":null,"StackTrace":null}


Now test the "Bye" method. Note there is no "body-json" in the request.
The name is passed in the "querystring".
We will later GET this method from the API Gateway.

{
	"class-type": "GreetingExample.GreetingHandler, GreetingExample",
	"method-name": "Bye",
	"method-param-type": "GreetingExample.GreetingRequest, GreetingExample",
	"params" : 
	{
		"querystring" : {"name" : "Joe"}
	}
}

It should return the following.

{"Greeting":"Bye Joe","Error":null,"StackTrace":null}


Now test "bad" input by passing in an empty request.

{}

It should return the following.



8. Configure API Gateway to call GreetingExample's "Hello" and "Bye" methods in the lambda function.
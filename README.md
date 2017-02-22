# AWS-api-to-lambda (.Net Core)

# PROBLEM  

- I don't want to write multiple AWS Lambda functions to handle different 
web requests from the AWS API Gateway.

# SOLUTION

- This framework routes requests received by AWS API Gateway to a specified .Net class method in a single AWS Lambda function for processing.

- This framework allows a single lambda function to handle different request types
based on the API Gateway endpoint configuration.
The request forwarding/processing information in provided declaratively in 
the AWS API Gateway configuration using templates.

- The framework also takes care of deserializing requests into .Net request 
objects for processing by the AWS lambda function request handlers.


# EXAMPLE - GreetingExample

## OBJECTIVE

- Create a single lambda function that will receive "hello" and "bye" requests from the AWS API Gateway and return a greeting response.

- In API Gateway we will integrate with this lambda function.

We will POST a "name" for "Hello" and 

GET a "Bye" by passing "name" as a querystring parameter.


## STEP 1: CREATE AWS LAMBDA FUNCTION

1) Create a new project

AWS lambda | AWS lambda Project (.Net Core) | Empty function


2) Add project reference to "AWSApiToLambdaLib"


3) Edit "AWS-lambda-tools-defaults.json"

Change 
```
"function-handler" : "GreetingExample::GreetingExample.Function::FunctionHandler"
```
to
```
"function-handler" : "AWSApiToLambdaLib::AWSApiToLambdaLib.Function::FunctionHandler"
```

4) Delete "Function.cs" file from GreetingExample Project


5) Create the following three classes that will handle the request (request class, response class and a  request handler class)

```
// GreetingRequest.cs
public class GreetingRequest : ApiGatewayInput
{
    public string Name { get; set; }
}
```

```
// GreetingResponse.cs
public class GreetingResponse
{
    public string Greeting { get; set; }

    public string Error { get; set; }

    public string StackTrace { get; set; }
}
```

```
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
```

6) Build and deploy GreetingExample to AWS lambda

Make sure to add "Amazon.Lambda.Core" nuget package to GreetingExample project.

Also, add "AWSSDK.Core 3.3.8.1" or later nuget package to GreetingExample project.


7) Test the Lambda directly with the following request to the "Hello" method. 
We will later POST to this method from the API Gateway.
```
{
    "class-type": "GreetingExample.GreetingHandler, GreetingExample",
    "method-name": "Hello",
    "method-param-type": "GreetingExample.GreetingRequest, GreetingExample",
    "body-json": {
        "Name": "Joe"
    }
}
```

It should return the following.
```
{
    "Greeting" : "Hello Joe",
    "Error"    : null,
    "StackTrace" : null
}
```

Now test the "Bye" method. Note there is no "body-json" in the request.

The name is passed in the "querystring".

We will later GET this method from the API Gateway.
```
{
    "class-type": "GreetingExample.GreetingHandler, GreetingExample",
    "method-name": "Bye",
    "method-param-type": "GreetingExample.GreetingRequest, GreetingExample",
    "params": {
        "querystring": {
            "name": "Joe"
        }
    }
}
```
It should return the following.
```
{
    "Greeting" : "Bye Joe",
    "Error"    : null,
    "StackTrace" : null
}
```

Now test "bad" input by passing in an empty request.
```
{}
```
It should return the following.
```
{
    "Error" : "class-type not specified in input. Aws API Gateway not configured. Did you configure the application/json mapping template under 'Integration Request'?",
    "StackTrace" : "   at AwsApiToLambdaLib.Function.ResolveType(String typeString, String name)\n   at AwsApiToLambdaLib.Function.FunctionHandler(ApiGatewayInput input, ILambdaContext context)"
}
```

## STEP 2: INTEGRATE API GATEWAY WITH THE LAMBDA FUNCTION

Configure API Gateway to call GreetingExample's "Hello" and "Bye" methods in the lambda function.

1) Create API

API name: Greeting

Description: Say hello and bye

2) Create resources for "hello" and "bye" endpoints.

Actions | Create Resource

Resource Name: hello

Press Create Resource button

Go back to root of the API (click on \ in the resource tree)

Actions | Create Resource

Resource Name: bye

Press Create Resource button

3) Configure "hello" endpoint

Click on "hello" in the resource tree

Actions | Create Method | POST

Integration type: Lambda function

Choose appropriate Lambda Region

Lambda Function: GreetingExample

Press Save

Under /hello - POST - Method Execution, click on "Integration Request"

Expand "Body Mapping Templates"

Click on "Add mapping template"

Request body passthrough: When there are no templates defined (recommended)

Content-Type: application/json

Generate template: Method Request passthrough

Replace the template with following template 

(Note the template just adds three additional fields: class-type, method-name, method-param-type.)

```
##  See http://docs.aws.amazon.com/apigateway/latest/developerguide/api-gateway-mapping-template-reference.html
##  This template will pass through all parameters including path, querystring, header, stage variables, and context through to the integration endpoint via the body/payload
#set($allParams = $input.params())
{
"class-type": "GreetingExample.GreetingHandler, GreetingExample",
"method-name": "Hello",
"method-param-type": "GreetingExample.GreetingRequest, GreetingExample",
"body-json" : $input.json('$'),
"params" : {
#foreach($type in $allParams.keySet())
    #set($params = $allParams.get($type))
"$type" : {
    #foreach($paramName in $params.keySet())
    "$paramName" : "$util.escapeJavaScript($params.get($paramName))"
        #if($foreach.hasNext),#end
    #end
}
    #if($foreach.hasNext),#end
#end
},
"stage-variables" : {
#foreach($key in $stageVariables.keySet())
"$key" : "$util.escapeJavaScript($stageVariables.get($key))"
    #if($foreach.hasNext),#end
#end
},
"context" : {
    "account-id" : "$context.identity.accountId",
    "api-id" : "$context.apiId",
    "api-key" : "$context.identity.apiKey",
    "authorizer-principal-id" : "$context.authorizer.principalId",
    "caller" : "$context.identity.caller",
    "cognito-authentication-provider" : "$context.identity.cognitoAuthenticationProvider",
    "cognito-authentication-type" : "$context.identity.cognitoAuthenticationType",
    "cognito-identity-id" : "$context.identity.cognitoIdentityId",
    "cognito-identity-pool-id" : "$context.identity.cognitoIdentityPoolId",
    "http-method" : "$context.httpMethod",
    "stage" : "$context.stage",
    "source-ip" : "$context.identity.sourceIp",
    "user" : "$context.identity.user",
    "user-agent" : "$context.identity.userAgent",
    "user-arn" : "$context.identity.userArn",
    "request-id" : "$context.requestId",
    "resource-id" : "$context.resourceId",
    "resource-path" : "$context.resourcePath"
    }
}
```

Press Save button.

4) Configure "bye" endpoint

Click on "bye" in the resource tree

Actions | Create Method | GET

Integration type: Lambda function

Choose appropriate Lambda Region

Lambda Function: GreetingExample

Press Save

Under /bye - POST - Method Execution, click on "Integration Request"

Expand "Body Mapping Templates"

Click on "Add mapping template"

Request body passthrough: When there are no templates defined (recommended)

Content-Type: application/json

Generate template: Method Request passthrough

Replace the template with following template 

(Note the template just adds three additional fields: class-type, method-name, method-param-type.

Also, it omits "body-json" since we will performing a GET operation with a querystring value.)

```
##  See http://docs.aws.amazon.com/apigateway/latest/developerguide/api-gateway-mapping-template-reference.html
##  This template will pass through all parameters including path, querystring, header, stage variables, and context through to the integration endpoint via the body/payload
#set($allParams = $input.params())
{
"class-type": "GreetingExample.GreetingHandler, GreetingExample",
"method-name": "Bye",
"method-param-type": "GreetingExample.GreetingRequest, GreetingExample",
"params" : {
#foreach($type in $allParams.keySet())
    #set($params = $allParams.get($type))
"$type" : {
    #foreach($paramName in $params.keySet())
    "$paramName" : "$util.escapeJavaScript($params.get($paramName))"
        #if($foreach.hasNext),#end
    #end
}
    #if($foreach.hasNext),#end
#end
},
"stage-variables" : {
#foreach($key in $stageVariables.keySet())
"$key" : "$util.escapeJavaScript($stageVariables.get($key))"
    #if($foreach.hasNext),#end
#end
},
"context" : {
    "account-id" : "$context.identity.accountId",
    "api-id" : "$context.apiId",
    "api-key" : "$context.identity.apiKey",
    "authorizer-principal-id" : "$context.authorizer.principalId",
    "caller" : "$context.identity.caller",
    "cognito-authentication-provider" : "$context.identity.cognitoAuthenticationProvider",
    "cognito-authentication-type" : "$context.identity.cognitoAuthenticationType",
    "cognito-identity-id" : "$context.identity.cognitoIdentityId",
    "cognito-identity-pool-id" : "$context.identity.cognitoIdentityPoolId",
    "http-method" : "$context.httpMethod",
    "stage" : "$context.stage",
    "source-ip" : "$context.identity.sourceIp",
    "user" : "$context.identity.user",
    "user-agent" : "$context.identity.userAgent",
    "user-arn" : "$context.identity.userArn",
    "request-id" : "$context.requestId",
    "resource-id" : "$context.resourceId",
    "resource-path" : "$context.resourcePath"
    }
}
```

Press Save button.

5) Deploy API

Click on "Greetings" in the left-most panel

Actions | Deploy API

Deployment stage: [New Stage]

Stage name: Prod

Press Deploy button

Press Save changes button at the top of the "Prod Stage Editor" screen.

Copy the "Invoke URL" displayed at the top the "Prod Stage Editor" screen.

```
https://xxxxxxx.execute-api.xx-xxxx-x.amazonaws.com/Prod
```

6) Test "Hello" endpoint of the "Greeting" API

Using Fiddler, Postman, cURL or similar
```
POST to https://xxxxxxx.execute-api.xx-xxxx-x.amazonaws.com/Prod/hello
```
For Content-Type header, specify application/json;charset=UTF-8
For body, send the following content:
```
{
    "Name": "Joe"
}
```
The POST should return the following response.
```
{
  "Greeting": "Hello Joe",
  "Error": null,
  "StackTrace": null
}
```

7) Test "Bye" endpoint of the "Greeting" API
```
GET https://xxxxxxx.execute-api.xx-xxxx-x.amazonaws.com/Prod/bye?name=Joe
```
Simply issue the above request in any browser or use issue a GET with Fiddler, Postman or cURL. 

You should see the following response.
```
{
  "Greeting": "Bye Joe",
  "Error": null,
  "StackTrace": null
}
```

##SUMMARY

- We have exposed two endpoints in the API Gateway.

- The requests received by these endpoints are handled by a single lambda function.

- The request transformation and processing information is provided in the "Integration Request" of the API Gateway.

- We used POST (for hello) and GET (for bye).

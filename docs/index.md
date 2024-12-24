# <a name="gettingStarted">Hello World</a>

```c#
using idunno.Bluesky.

using (BlueskyAgent agent = new ())
{
    var loginResult = await agent.Login(username, password);
    if (loginResult.Succeeded)
    {
        var response = 
            await agent.Post("Hello World");

       if (response.Succeeded)
       {
          // It worked.
       }
   }
}
```
## <a name="makingRequests">Making requests to Bluesky</a>

All the supported Bluesky operations are contained in the `BlueskyAgent` class, which takes care of session management for you.

Most requests to Bluesky are made over [HTTP](https://docs.bsky.app/docs/category/http-reference) and the results are wrapped up in an `HttpResult` instance,
which contains the HTTP status code returned by the API, and any result or error returned.

## <a name="understandingResults">Understanding results & error handling</a>
Every API call through an agent returns an `AtProtoHttpResult<T>`. This approach, which you may recognize from ASP.NET,
avoids the use of exceptions should the HTTP call fail, and allows you to view any extra error information the Bluesky APIs may return.

If a request is successful the API return value will be in the `Result` property of the `AtProtoHttpResult<T>` and
the `Succeeded` property will be `true`. The `StatusCode` property will be `HttpStatusCode.OK`.

If a request has failed, either at the HTTP or the API layer then the `Succeeded` property will be `false`, and
the `Result` property will likely be `null`. You can check the `StatusCode` property to see what HTTP status code was
encountered during the API call, and, if the API call reached the API endpoint the `Error` property will contain any
error message returned by the endpoint.

For example, a login call returns an `AtProtoHttpResult<bool>`. To check the operation succeeded you would

1. Check the that the `Succeeded` property is true, which indicates the underlying request returned a `HttpStatusCode.OK` status code, and an available result.
2. If `Succeeded` is `true`, you can use the `Result` property and continue on your way.
   If `Succeeded` is `false` you can use the `StatusCode` property to examine the HTTP status code returned by the API and,
1.if the API has given a detailed error response, you can use the `Error` property to view any extended error information returned, which may have an `Error` and a `Message` set.
3. If the `StatusCode` property is `HttpStatusCode.OK` then the API call succeeded but no result was returned, which shouldn't happen.

When calling an API you should use the following pattern to check for errors.

```c#
// Make an API call to get the timeline for the current user.
var timelineResult = await agent.GetTimeline();

if (timelineResult.Succeeded)
{
    // Everything was successful, continue on with your code
}
else
{
    Console.WriteLine($"getTimelineResult failed: {timelineResult.StatusCode}.");
    Console.Write($"\t{timelineResult.Error.Error} : ");
    Console.WriteLine($"\t{timelineResult.Error.Message}");
}
```

The `EnsureSucceeded()` method on `AtProtoHttpResult<T>` will throw and `AtProtoHttpRequestException` if the `Succeeded` property is false.

## <a name="connecting">Connecting to Bluesky</a>

As you can see from the [Hello World](#gettingStarted) example connecting to Bluesky consists of creating an instance of a `BlueskyAgent` and then calling the login method.

```c#
using (BlueskyAgent agent = new ())
{
    var loginResult =  await agent.Login(username, password);
    if (loginResult.Succeeded && agent.Session is not null)
    {
        // Do your Bluesky thing
    }
}
```

When a login is successful the agent will store the information needed for subsequent API calls in its `Session` property.
API calls that require authentication will use this information and the access tokens will, by default, refresh automatically.

If a user has email based two-factor authentication logins need an extra step.
The first login attempt will fail with an `AuthFactorTokenRequired` error, at which point you should prompt the user to enter their sign-in code,
and call `Login` again, this time with the username, password and the sign-in code.

```c#
var loginResult = await agent.Login(username, password);

if (!loginResult.Succeeded &&
    string.Equals(
        loginResult.Error.Error!, 
        "AuthFactorTokenRequired", 
        StringComparison.OrdinalIgnoreCase))
{
    Console.WriteLine("Account requires an email authentication code.");
    Console.WriteLine("Enter the email authentication code or press return to exit:");
    string? emailAuthenticationCode = Console.ReadLine();

    if (!string.IsNullOrEmpty(emailAuthenticationCode))
    {
        // Try again with the auth code.
        loginResult = await agent.Login(username, password, emailAuthenticationCode);
    }
}
```

Note that app passwords don't currently require authentication codes.

Some users run their own Personal Data Servers, and/or have an [decentralized identifier](https://www.w3.org/TR/did-core/) (DID) that isn't part
of the [directory](https://web.plc.directory/) Bluesky runs. Whilst you can simply ask a user for their PDS location
you can also use the `ResolveHandle` method in the `AtProtoAgent` class to resolve a handle to a DID, and then use the
`ResolveDIDDocument` method in the `DirectoryServer` to discover the location of their PDS. Once you have a user's PDS all write and delete operations
should be performed against that PDS. The `AtProtoAgent` `Login` method does this behind the scenes so you don't have to.

### <a name="usingAProxy">Using a proxy server<a>

The agent constructors can take an `HttpClient` if you want to customize agent headers or other http properties.
This can be useful if you want to use a proxy server. The agent classes contain a helper method,
`CreateConfiguredHttpClient()` which will create an HttpClient configured to use a proxy server,
and/or set a custom user agent string. You can then pass this HttpClient to the agent constructor.

Note that if you specify a proxy URI the HttpClient returned will be configured to not check Certificate Revocation lists as typically the
HTTPS certificates created by proxy servers like Fiddler or Burp Suite don't have CRL endpoints.

```c#

HttpClient httpClient = Agent.CreateHttpClientForProxy("http://localhost:8866", "mydotnetclient/1.0");
BlueskyAgent agent = new (httpClient)
{
    // And now this instance of agent will use your handler.
}
```

If you are creating your own HttpClient instance you will need to ensure the handler you create uses has compression support enabled.

```c#
HttpClientHandler handler = new ()
{
    AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate
};
HttpClient httpClient = new (handler);

BlueskyAgent agent = new (httpClient)
{
    // And now this instance of agent will use your handler.
}
```

### <a name="disablingTokenRefresh">Disabling token refresh</a>

If you want to disable automatic token refresh in an agent you can do that by the `EnableTokenRefresh` property in options to false.

```c#
var options = new BlueskyAgentOptions() { EnableBackgroundTokenRefresh = false };

using (BlueskyAgent agent = new (options)
{   
    var did = await agent.ResolveHandle("blowdart.me");
}
```
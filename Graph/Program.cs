using Microsoft.Graph;
using Microsoft.Graph.Auth;
using Microsoft.Identity.Client;

var clientId = "";
var tenantId = "";

var app = PublicClientApplicationBuilder
    .Create(clientId)
    .WithAuthority(AzureCloudInstance.AzurePublic, tenantId)
    .WithRedirectUri("http://localhost")
    .Build();

string[] scopes = { "user.read" };
var result = await app.AcquireTokenInteractive(scopes).ExecuteAsync();

var provider = new InteractiveAuthenticationProvider(app, scopes);
var client = new GraphServiceClient(provider);

User me = await client.Me.Request().GetAsync();
Console.WriteLine($"Display name: {me.DisplayName}");
// Set the parameters for the Google authentication flow
var initializer = new GoogleAuthorizationCodeFlow.Initializer
{
    ClientSecrets = new ClientSecrets
    {
        ClientId = "229651248563-nbbvsh5i5hrqgb6stab4vd2kpoi2vmt3.apps.googleusercontent.com",
        ClientSecret = "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"
    },
    Scopes = new[] { DriveService.Scope.Drive },
    DataStore = new FileDataStore("Drive.Auth.Store")
};

// Create the authorization flow
var flow = new GoogleAuthorizationCodeFlow(initializer);

// Exchange the authorization code for an access token
var code = Request["code"];
var token = flow.ExchangeCodeForTokenAsync("user", code, "http://localhost:7284/Callback.aspx", CancellationToken.None).Result;

// Create the Drive API service
var service = new DriveService(new BaseClientService.Initializer
{
    HttpClientInitializer = token,
    ApplicationName = "Your application name here"
});

// Use the Drive API service to perform authenticated actions
// For example:
var fileList = service.Files.List().Execute().Files;

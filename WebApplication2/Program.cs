using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using WebApplication2.Data;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Auth.OAuth2.Flows;
using Google.Apis.Auth.OAuth2.Web;
using Google.Apis.Services;
using Google.Apis.Drive.v3;
using Google.Apis.Util.Store;
using static Google.Apis.Requests.BatchRequest;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString));
builder.Services.AddDatabaseDeveloperPageExceptionFilter();

builder.Services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = true)
    .AddEntityFrameworkStores<ApplicationDbContext>();
builder.Services.AddRazorPages();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseMigrationsEndPoint();
}
else
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}
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

// Generate the authorization URL
var url = new AuthorizationCodeWebApp(flow, "http://localhost:7284/Callback.aspx", "http://localhost:7284").GetAuthorizationUrl(null);

// Redirect the user to the authorization URL
HttpResponse.Redirect(url);

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapRazorPages();

app.Run();


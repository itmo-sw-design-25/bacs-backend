using BaCS.Presentation.BusinessProcess.Extensions;
using Elsa.Extensions;
using Microsoft.AspNetCore.Mvc;

var builder = WebApplication.CreateBuilder(args);

builder.WebHost.UseStaticWebAssets();

var services = builder.Services;
var configuration = builder.Configuration;
var environment = builder.Environment;

services.AddElsaWorkflowsBusinessProcess(configuration, environment);

services.AddCors(cors =>
    cors.AddDefaultPolicy(policy => policy
        .AllowAnyHeader()
        .AllowAnyMethod()
        .AllowAnyOrigin()
        .WithExposedHeaders("*")
    )
);

services.AddRazorPages(options => options.Conventions
    .ConfigureFilter(new IgnoreAntiforgeryTokenAttribute())
);

var app = builder.Build();

app.UsePathBase("/elsaworkflows");

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

app
    .UseHttpsRedirection()
    .UseBlazorFrameworkFiles()
    .UseRouting()
    .UseCors()
    .UseStaticFiles();

app
    .UseAuthentication()
    .UseAuthorization();

app
    .UseWorkflowsApi()
    .UseWorkflows();

app.MapFallbackToPage("/_Host");

await app.RunAsync();

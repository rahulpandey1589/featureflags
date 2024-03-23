using Carter;
using featureflags.OptionSetup;
using featureflags.Ulititles;
using Microsoft.FeatureManagement;

var builder = WebApplication.CreateBuilder(args);

builder.Services.ConfigureOptions<ApplicationSettingsOptionSetup>();

builder.Services
    .AddEndpointsApiExplorer()
    .AddSwaggerGen()
    .AddCarter()
    .AddFeatureManagement();

builder.Services.AddHttpClient(
    ApplicationConstants.RegresApi, httpClient =>
{
    httpClient.BaseAddress = new Uri("https://reqres.in/api");
});


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapCarter();
app.UseHttpsRedirection();
app.Run();
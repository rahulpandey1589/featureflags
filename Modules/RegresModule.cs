using Carter;
using System.Text.Json;
using featureflags.Models;
using featureflags.Ulititles;
using Microsoft.FeatureManagement;

namespace featureflags.Modules;

public class RegresModule : ICarterModule
{
    private readonly IFeatureManager _featureManager;
    private readonly IHttpClientFactory _clientFactory;
    
    public RegresModule(
        IFeatureManager featureManager,
        IHttpClientFactory clientFactory)
    {
        _featureManager = featureManager;
        _clientFactory = clientFactory;
    }


    public void AddRoutes(
        IEndpointRouteBuilder app)
    {
        app.MapGet("/users", GetUsers);
    }


    private async Task<bool> IsEnabled() =>
        await _featureManager.IsEnabledAsync(ApplicationConstants.EnableRegresApi);


    public async Task<Root?> GetUsers(
    )
    {
        bool isRegresEnabled = await IsEnabled();

        if (isRegresEnabled)
        {
            return await CallRegresApi();
        }

        return await Task.FromResult(new Root());
    }
    
    private async Task<Root?> CallRegresApi(
    )
    {
        var httpClient = _clientFactory.CreateClient(ApplicationConstants.RegresApi);
        var httpResponseMessage = await httpClient.GetAsync("users?page=1");

        if (httpResponseMessage.IsSuccessStatusCode)
        {
            await using var contentStream =
                await httpResponseMessage.Content.ReadAsStreamAsync();

            return await JsonSerializer.DeserializeAsync<Root>(contentStream);
        }
        return await Task.FromResult(new Root());
    }
}
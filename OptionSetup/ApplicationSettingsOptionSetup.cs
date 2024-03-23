using featureflags.Models;
using Microsoft.Extensions.Options;

namespace featureflags.OptionSetup;

public class ApplicationSettingsOptionSetup : IConfigureOptions<ApplicationSettings>
{
    private readonly IConfiguration _configuration;

    public ApplicationSettingsOptionSetup(
        IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public void Configure(
        ApplicationSettings options)
    {
        _configuration.GetSection(nameof(ApplicationSettings)).Bind(options);
    }
}
using Discord;
using Discord.Interactions;
using System.Reflection;

namespace Chaser;

/// <summary>
/// Represents an attribute used to validate an image property.
/// </summary>
public partial class ImageAttribute : PropertyPreconditionAttribute
{
    public override async Task<PreconditionResult> CheckRequirementsAsync(IInteractionContext context,
        PropertyInfo propertyInfo, object obj, string value, IServiceProvider services)
    {
        if (!Uri.IsWellFormedUriString(value, UriKind.Absolute))
            return PreconditionResult.FromError($"The string '{value}' isn't a well formed URL!");

        string websiteMarkdownUrl = Format.Url("website", value);
        HttpClient http = services.GetRequiredService<HttpClient>();
        HttpResponseMessage response = await http.GetAsync(value);

        if (!response.IsSuccessStatusCode)
            return PreconditionResult.FromError($"Unfortunately, I can't access this {websiteMarkdownUrl}.");

        if (!response.Content.Headers.ContentType.MediaType.StartsWith("image/", StringComparison.OrdinalIgnoreCase))
            return PreconditionResult.FromError($"Uhm… I can't detect an image on this {websiteMarkdownUrl}!");
        
        return PreconditionResult.FromSuccess();
    }
}

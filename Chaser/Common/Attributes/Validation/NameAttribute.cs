using System.Reflection;
using Chaser.Core;
using Discord;
using Discord.Interactions;
using Humanizer;

namespace Chaser;

/// <summary>
/// Represents an attribute used to validate a character name property.
/// </summary>
public class NameAttribute : PropertyPreconditionAttribute
{
    public override async Task<PreconditionResult> CheckRequirementsAsync(IInteractionContext context, PropertyInfo propertyInfo, object obj, string value, IServiceProvider services)
    {
        string name = value.Transform(To.TitleCase);
        Uri uri = new(RucoyCDN.GetCharacterUrl(name));
        HttpClient http = services.GetRequiredService<HttpClient>();
        HttpResponseMessage response = await http.GetAsync(uri);

        if (response?.RequestMessage?.RequestUri != uri)
        {
            string url = Format.Url("Rucoy Online", RucoyConfig.BaseUrl);
            return PreconditionResult.FromError($"{Format.Bold(name)} isn't a player in the world of the {url}!");
        }

        propertyInfo.SetValue(obj, name);
        return PreconditionResult.FromSuccess();
    }
}
